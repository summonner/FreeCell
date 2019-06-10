using System.Linq;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public class CardMover {
		private readonly IBoardController board;
		public CardMover( IBoardController board ) {
			this.board = board;
		}

		private IPile selectedPile;
		private Card[] poped = null;
		private NumberOfMovables numMovables;

		public Card clicked {
			get {
				return poped.FirstOrDefault();
			}
		}

		public bool SetSource( PositionOnBoard selected ) {
			if ( selected.type == PileId.Type.Home ) {
				return false;
			}

			numMovables = new NumberOfMovables( board.AsReadOnly() );	// have to create before pop
			selectedPile = board[selected.pile];
			poped = selectedPile.Pop( selected.row );
			if ( poped.IsNullOrEmpty() == true ) {
				return false;
			}

			return true;
		}

		public bool Execute( IEnumerable<PileId> destinations ) {
			if ( poped.IsNullOrEmpty() == true ) {
				return false;
			}

			var piles = GetPiles( destinations );
			var pile = TryMove( piles );
			if ( pile == null ) {
				selectedPile.Push( poped );
				InGameEvents.CannotMove( poped );
				return false;
			}
			else {
				pile.Push( poped );
				InGameEvents.MoveCards( poped, selectedPile.id, pile.id );
				return true;
			}
		}

		private IEnumerable<IPile> GetPiles( IEnumerable<PileId> ids ) {
			foreach ( var id in ids ) {
				yield return board[id];
			}
		}

		private IPile TryMove( IEnumerable<IPile> piles ) {
			if ( poped.Length > numMovables.value ) {
				return null;
			}

			foreach ( var pile in piles ) {
				if ( CanMove( poped, pile, numMovables ) == false ) {
					continue;
				}

				return pile;
			}

			return null;
		}

		public static bool CanMove( Card[] cards, IPileLookup to, NumberOfMovables numMovables ) {
			return to.IsAcceptable( cards )
				&& cards.Length <= numMovables.MoveTo( to );
		}
	}
}