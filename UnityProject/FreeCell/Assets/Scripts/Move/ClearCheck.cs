using UnityEngine;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public class ClearCheck : System.IDisposable {
		private IBoardLookup board;

		public ClearCheck( IBoardLookup board ) {
			this.board = board;
			InGameEvents.OnMoveCards += OnMoveCards;
		}

		public void Dispose() {
			InGameEvents.OnMoveCards -= OnMoveCards;
		}

		public void OnMoveCards( IEnumerable<Card> cards, PileId from, PileId to ) {
			var homes = board.Traverse( PileId.Type.Home );

			foreach ( var home in homes ) {
				if ( IsClear( home ) == false ) {
					return;
				}
			}

			InGameEvents.Clear();
		}

		private static bool IsClear( IList<Card> suit ) {
			if ( suit.Count != 13 ) {
				return false;
			}

			var prev = Card.Blank;
			foreach ( var card in suit ) {
				if ( IsValid( prev, card ) == false ) {
					return false;
				}
				prev = card;
			}
			return true;
		}

		public static bool IsValid( Card first, Card next ) {
			if ( first == Card.Blank ) {
				return next.rank == Card.Rank.Ace;
			}

			return first.suit == next.suit
				&& (first.rank + 1) == next.rank;
		}
	}
}