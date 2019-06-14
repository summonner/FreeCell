using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public class PossibleMoveFinder {
		private readonly IBoardLookup board;
		private readonly MoveTester mover;
		public PossibleMoveFinder( IBoardLookup board ) {
			this.board = board;
			this.mover = new MoveTester( board );
		}

		public bool HasAnyMove() {
			var numMovables = new NumberOfMovables( board );
			if ( numMovables.value > 1 ) {
				return true;
			}

			return FindMoves().Any();
		}

		public IEnumerable<Move> FindMoves() {
			return FindMoves( PileId.Type.Home, PileId.Type.Free, PileId.Type.Table );
		}

		public IEnumerable<Move> FindMoves( params PileId.Type[] destinations ) {
			var fromPiles = board[PileId.Type.Table, PileId.Type.Free].Where( (pile) => ( pile.Count > 0 ) );
			var toPiles = board[destinations];
			var maxAcceptable = GetMaxAcceptables( destinations );

			foreach ( var from in fromPiles ) {
				foreach ( var position in GetPositions( from, maxAcceptable ) ) {
					if ( mover.SetSource( position ) != MoveTester.Result.Success ) {
						break;
					}

					foreach ( var to in toPiles ) {
						if ( to == from ) {
							continue;
						}

						if ( position.type == PileId.Type.Free && to.id.type == PileId.Type.Free ) {
							continue;
						}

						if ( mover.SetDestination( to.id ) == MoveTester.Result.Success ) {
							yield return new Move( mover );
						}
					}
				}
			}
		}

		private IEnumerable<PositionOnBoard> GetPositions( IPileLookup from, int maxMovables ) {
			for ( int i = from.Count - 1; i >= Mathf.Max( 0 , from.Count - maxMovables ); --i ) {
				yield return new PositionOnBoard( from.id, i );
			}
		}

		private int GetMaxAcceptables( PileId.Type[] destination ) {
			if ( destination.Contains( PileId.Type.Table ) == true ) {
				return int.MaxValue;
			}
			else {
				return 1;
			}
		}
	}

	public struct Move {
		public readonly Card[] cards;
		public readonly PileId from;
		public readonly PileId to;

		public Move( MoveTester tester ) {
			this.cards = tester.subjects;
			this.from = tester.selected.pile;
			this.to = tester.destination;
		}

		public override string ToString() {
			return cards.Join() + " : " + from + " -> " + to;
		}
	}
}