using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public class PossibleMoveFinder : IRuleComponent {
		private readonly IBoardLookup board;
		public PossibleMoveFinder( IBoardLookup board ) {
			this.board = board;

			InGameEvents.OnMoveCards += FindMove;
		}

		public void Dispose() {
			InGameEvents.OnMoveCards -= FindMove;
		}

		public void Reset() {
			// do nothing
		}

		public void FindMove( IEnumerable<Card> _does, PileId _not, PileId _use ) {
			var fromPiles = board[PileId.Type.Table, PileId.Type.Free];
			var toPiles = board[PileId.Type.Table, PileId.Type.Free, PileId.Type.Home];

			var numMovables = new NumberOfMovables( board );
			foreach ( var from in fromPiles ) {
				foreach ( var to in toPiles ) {
					if ( to == from ) {
						continue;
					}

					foreach ( var cards in FindMovables( from, numMovables ) ) {
						if ( CardMover.CanMove( cards, to, numMovables ) == true ) {
							return;
						}
					}
				}
			}

			InGameEvents.NoMoreMoves();
		}

		private IEnumerable<Card[]> FindMovables( IPileLookup from, NumberOfMovables numMovables ) {
			for ( int numMoves = 1; numMoves <= numMovables.value; ++numMoves ) {
				int i = from.Count - numMoves;
				if ( i < 0 ) {
					break;
				}

				if ( from.CanMove( i ) == false ) {
					break;
				}

				yield return from.Skip( i ).ToArray();
			}
		}
	}
}