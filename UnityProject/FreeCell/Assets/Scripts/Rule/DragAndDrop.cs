using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public class DragAndDrop : IRuleComponent, IDragAndDropListener {
		private readonly IBoardController board;

		public DragAndDrop( IBoardController board ) {
			this.board = board;
			Util.Event.SubscribeHelper.Subscribe( this );
		}

		public void Dispose() {
			Util.Event.SubscribeHelper.Unsubscribe( this );
		}

		public void Reset() {
			// do nothing
		}

		private PositionOnBoard selected;
		private IEnumerable<Card> selectedCards = null;
		void IDragAndDropListener.OnBeginDrag( PositionOnBoard position ) {
			var pile = board[position.pile];
			if ( pile.CanMove( position.row ) == false ) {
				return;
			}

			selected = position;
			selectedCards = pile.Skip( position.row );
			InGameEvents.BeginFloatCards( selectedCards );
		}

		void IDragAndDropListener.OnDrag( PositionOnBoard position, Vector3 displacement ) {
			if ( selectedCards == null ) {
				return;
			}

			if ( selected != position ) {
				return;
			}

			InGameEvents.FloatCards( selectedCards, displacement );
		}

		void IDragAndDropListener.OnEndDrag( PositionOnBoard position ) {
			if ( selectedCards == null ) {
				return;
			}

			if ( selected != position ) {
				return;
			}

			InGameEvents.EndFloatCards( selectedCards );
			selectedCards = null;
		}

		void IDragAndDropListener.OnDrop( PositionOnBoard position, IEnumerable<PileId> receivers ) {
			if ( selectedCards == null ) {
				return;
			}

			if ( selected != position ) {
				return;
			}

			var numMovable = board.CountMaxMovables();
			var source = board[position.pile];
			var poped = source.Pop( position.row );
			foreach ( var destination in receivers ) {
				var dest = board[destination];
				if ( CanMoveCards( poped, dest, numMovable ) == true ) {
					dest.Push( poped );
					InGameEvents.MoveCards( poped, position.pile, destination );
					selectedCards = null;
					return;
				}
			}
			source.Push( poped );
		}

		private bool CanMoveCards( Card[] poped, IPile dest, int numMovable ) {
			if ( poped.IsNullOrEmpty() == true ) {
				return false;
			}

			if ( dest.IsAcceptable( poped ) == false ) {
				return false;
			}

			var adjustment = AutoMove.IsEmptyTableau( dest ) ? 2 : 1;
			if ( poped.Length > numMovable / adjustment ) {
				return false;
			}

			return true;
		}
	}
}