using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public class PossibleMoveFinder : IRuleComponent {
		private readonly IBoardLookup board;
		private readonly MoveTester mover;
		public PossibleMoveFinder( IBoardLookup board ) {
			this.board = board;
			this.mover = new MoveTester( board );

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

			foreach ( var from in fromPiles ) {
				foreach ( var position in GetPositions( from ) ) {
					if ( mover.SetSource( position ) != MoveTester.Result.Success ) {
						break;
					}

					foreach ( var to in toPiles ) {
						if ( to == from ) {
							continue;
						}

						if ( mover.SetDestination( to.id ) == MoveTester.Result.Success ) {
							//Debug.Log( position + " -> " + to.id + "\n\n" + board );
							return;
						}
					}
				}
			}

			InGameEvents.NoMoreMoves();
		}

		private IEnumerable<PositionOnBoard> GetPositions( IPileLookup from ) {
			for ( int i = from.Count - 1; i >= 0; --i ) {
				yield return new PositionOnBoard( from.id, i );
			}
		}
	}
}