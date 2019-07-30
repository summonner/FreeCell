using System.Linq;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public class CardMover {
		private readonly IBoardController board;
		private readonly MoveTester tester;
		public CardMover( IBoardController board ) {
			this.board = board;
			this.tester = new MoveTester( board.AsReadOnly() );
		}

		public Card clicked {
			get {
				return tester.subjects.FirstOrDefault();
			}
		}

		public bool SetSource( PositionOnBoard selected ) {
			return tester.SetSource( selected ) != MoveTester.Result.ImmovableCardSelected;
		}

		public bool ExecuteAndResult( IEnumerable<PileId> destinations ) {
			bool notEnoughFreeCell = false;
			foreach ( var pile in destinations ) {
				switch ( tester.SetDestination( pile ) ) {
					case MoveTester.Result.ImmovableCardSelected:
						return false;

					case MoveTester.Result.CouldnotFindDestination:
						continue;

					case MoveTester.Result.NotEnoughFreeCell:
						notEnoughFreeCell = true;
						continue;

					case MoveTester.Result.Success:
						Move( tester.selected, tester.destination );
						return true;
				}
			}

			if ( notEnoughFreeCell == true ) {
				InGameEvents.NotEnoughFreeCells();
			}

			return false;
		}

		public void Execute( IEnumerable<PileId> destinations ) {
			if ( ExecuteAndResult( destinations ) == false ) {
				InGameEvents.CannotMove( tester.subjects );
			}
		}

		private void Move( PositionOnBoard selected, PileId destination ) {
			var poped = board[selected.pile].Pop( selected.row );
			board[destination].Push( poped );
			InGameEvents.MoveCards( poped, selected.pile, destination );
		}
	}
}