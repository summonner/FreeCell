using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public class MoveTester {
		public enum Result {
			Success,
			ImmovableCardSelected,
			NotEnoughFreeCell,
			CouldnotFindDestination,
		}

		private readonly IBoardLookup board;
		private NumberOfMovables numMovables;

		public MoveTester( IBoardLookup board ) {
			this.board = board;
		}

		public PositionOnBoard selected { get; private set; }
		public PileId destination { get; private set; }
		public Card[] subjects { get; private set; }

		public Result SetSource( PositionOnBoard selected ) {
			this.selected = selected;
			subjects = new Card[0];

			if ( selected.type == PileId.Type.Home ) {
				return Result.ImmovableCardSelected;
			}

			var pile = board[selected.pile];
			if ( pile.CanMove( selected.row ) == false ) {
				return Result.ImmovableCardSelected;
			}

			numMovables = new NumberOfMovables( board );
			subjects = pile.Skip( selected.row ).ToArray();
			if ( subjects.Length > numMovables.value ) {
				return Result.NotEnoughFreeCell;
			}

			return Result.Success;
		}

		public Result SetDestination( PileId destination ) {
			this.destination = destination;

			if ( subjects.IsNullOrEmpty() == true ) {
				return Result.ImmovableCardSelected;
			}

/*			if ( subjects.Length > numMovables.value ) {
				return Result.NotEnoughFreeCell;
			}*/

			var pile = board[destination];
			if ( pile.IsAcceptable( subjects ) == false ) {
				return Result.CouldnotFindDestination;
			}

			if ( subjects.Length > numMovables.MoveTo( pile ) ) {
				return Result.NotEnoughFreeCell;
			}

			return Result.Success;
		}
	}
}