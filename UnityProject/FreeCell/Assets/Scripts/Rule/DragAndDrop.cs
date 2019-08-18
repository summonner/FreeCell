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

		private IDictionary<int, IEnumerable<Card>> draggings = new Dictionary<int, IEnumerable<Card>>( 1 );
		void IDragAndDropListener.OnBeginDrag( int pointerId, PositionOnBoard position ) {
			if ( draggings.Count > 0 ) {
				return;
			}

			if ( draggings.ContainsKey( pointerId ) == true ) {
				return;
			}

			var pile = board[position.pile];
			if ( pile.CanMove( position.row ) == false ) {
				return;
			}

			var selectedCards = pile.Skip( position.row );
			draggings.Add( pointerId, selectedCards );
			InGameEvents.BeginFloatCards( selectedCards );
		}

		void IDragAndDropListener.OnDrag( int pointerId, Vector3 displacement ) {
			var selectedCards = GetSelectedCards( pointerId );
			if ( selectedCards == null ) {
				return;
			}

			InGameEvents.FloatCards( selectedCards, displacement );
		}

		void IDragAndDropListener.OnEndDrag( int pointerId ) {
			var selectedCards = GetSelectedCards( pointerId );
			if ( selectedCards == null ) {
				return;
			}
			
			InGameEvents.EndFloatCards( selectedCards );
			draggings.Remove( pointerId );
		}

		void IDragAndDropListener.OnDrop( int pointerId, PositionOnBoard position, IEnumerable<PileId> receivers ) {
			var selectedCards = GetSelectedCards( pointerId );
			if ( selectedCards == null ) {
				return;
			}

			if ( mover.SetSource( position ) == false ) {
				return;
			}
			
			if ( mover.ExecuteAndResult( receivers ) == true ) {
				draggings.Remove( pointerId );
			}
		}

		private IEnumerable<Card> GetSelectedCards( int pointerId ) {
			IEnumerable<Card> selectedCards = null;
			if ( draggings.TryGetValue( pointerId, out selectedCards ) == false ) {
				return null;
			}

			return selectedCards;
		}
	}
}