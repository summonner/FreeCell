using UnityEngine;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public class ClearCheck : IRuleComponent {
		private IBoardLookup board;

		public ClearCheck( IBoardLookup board ) {
			this.board = board;
			InGameEvents.OnMoveCards += OnMoveCards;
			InGameEvents.OnAutoPlay += OnMoveCards;
		}

		public void Dispose() {
			InGameEvents.OnMoveCards -= OnMoveCards;
			InGameEvents.OnAutoPlay -= OnMoveCards;
		}

		public void Reset() {
			// do nothing
		}

		public void OnMoveCards( IEnumerable<Card> cards, PileId from, PileId to ) {
			var piles = board[PileId.Type.Table, PileId.Type.Free];
			foreach ( var pile in piles ) {
				if ( pile.IsNullOrEmpty() == false ) {
					return;
				}
			}

			InGameEvents.Clear();
		}
	}
}