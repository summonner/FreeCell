using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public class DragAndDrop : IRuleComponent, IDragAndDropListener {
		private readonly IBoardController board;
		private readonly CardMover mover;

		public DragAndDrop( IBoardController board ) {
			this.board = board;
			this.mover = new CardMover( board );

			PlayerInputEvents.Subscribe( this );
		}

		public void Dispose() {
			PlayerInputEvents.Unsubscribe( this );
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

			if ( mover.SetSource( position ) == false ) {
				return;
			}
			
			if ( mover.Execute( receivers ) == true ) {
				selectedCards = null;
			}
		}
	}
}