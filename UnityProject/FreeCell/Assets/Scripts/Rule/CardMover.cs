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

		public bool Execute( IEnumerable<IPile> destinations ) {
			if ( poped.IsNullOrEmpty() == true ) {
				return false;
			}

			var result = TryMove( destinations );
			if ( result == false ) {
				selectedPile.Push( poped );
				InGameEvents.CannotMove( poped );
			}

			return result;
		}

		private bool TryMove( IEnumerable<IPile> piles ) {
			if ( poped.Length > numMovables.value ) {
				return false;
			}

			foreach ( var pile in piles ) {
				if ( pile.IsAcceptable( poped ) == false ) {
					continue;
				}

				if ( poped.Length > numMovables.MoveTo( pile ) ) {
					continue;
				}

				pile.Push( poped );
				InGameEvents.MoveCards( poped, selectedPile.id, pile.id );
				return true;
			}

			return false;
		}
	}
}