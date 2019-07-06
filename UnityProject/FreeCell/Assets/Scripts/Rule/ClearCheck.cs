using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public class ClearCheck : IRuleComponent {
		private readonly IBoardLookup board;
		private bool isCleared = false;

		public ClearCheck( IBoardLookup board ) {
			this.board = board;
			InGameEvents.OnPlayerMove += OnMoveCards;
			InGameEvents.OnGameClear += OnClear;
			InGameEvents.OnNewGame += OnNewGame;
		}

		public void Dispose() {
			InGameEvents.OnPlayerMove -= OnMoveCards;
			InGameEvents.OnGameClear -= OnClear;
			InGameEvents.OnNewGame -= OnNewGame;
		}

		public void Reset() {
			// do nothing
		}

		public void OnMoveCards( IEnumerable<Card> cards, PileId from, PileId to ) {
			if ( isCleared == true ) {
				return;
			}

			var piles = board[PileId.Type.Table, PileId.Type.Free];
			foreach ( var pile in piles ) {
				if ( IsSortedByDecendingRank( pile ) == false ) {
					return;
				}
			}

			InGameEvents.GameClear();
		}

		private bool IsSortedByDecendingRank( IPileLookup pile ) {
			for ( int i=0; i < pile.Count - 1; ++i ) {
				if ( pile[i].rank < pile[i + 1].rank ) {
					return false;
				}
			}
			return true;
		}

		private void OnNewGame( StageNumber _ ) {
			isCleared = false;
		}

		private void OnClear() {
			isCleared = true;
		}
	}
}