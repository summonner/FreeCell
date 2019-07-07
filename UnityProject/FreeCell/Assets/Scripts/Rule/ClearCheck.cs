using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public class ClearCheck : IRuleComponent {
		private readonly IBoardLookup board;

		public ClearCheck( IBoardLookup board ) {
			this.board = board;
			Register();
			InGameEvents.OnGameClear += Unregister;
			InGameEvents.OnNewGame += Register;
		}

		public void Dispose() {
			Unregister();
			InGameEvents.OnGameClear -= Unregister;
			InGameEvents.OnNewGame -= Register;
		}

		public void Reset() {
			// do nothing
		}

		private void Register( StageNumber _ ) {
			Register();
		}

		private void Register() {
			InGameEvents.OnMoveCards += OnMoveCards;
		}

		private void Unregister() {
			InGameEvents.OnMoveCards -= OnMoveCards;
		}

		public void OnMoveCards( IEnumerable<Card> cards, PileId from, PileId to ) {
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
	}
}