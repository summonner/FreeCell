using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public class ClearCheck : IRuleComponent {
		private IBoardLookup board;

		public ClearCheck( IBoardLookup board ) {
			this.board = board;
			InGameEvents.OnPlayerMove += OnMoveCards;
		}

		public void Dispose() {
			InGameEvents.OnPlayerMove -= OnMoveCards;
		}

		public void Reset() {
			// do nothing
		}

		public void OnMoveCards( IEnumerable<Card> cards, PileId from, PileId to ) {
			FindDeadLock();
		}

		private IDictionary<Card, Card> remains = new SortedList<Card, Card>( 52, new DecendingRank() );
		private Stack<Card> stack = new Stack<Card>();
		private void FindDeadLock() {
			Init();

			while ( remains.IsNullOrEmpty() == false ) {
				stack.Clear();
				var card = remains.First().Key;
				if ( AddStack( card ) == false ) {
					return;
				}

				while ( stack.Count > 0 ) {
					card = GetLowerRank( stack.Peek() );
					if ( remains.ContainsKey( card ) == false ) {
						remains.Remove( stack.Pop() );
						continue;
					}

					if ( AddStack( card ) == false ) {
						return;
					}
				}
			}

			InGameEvents.GameClear();
		}

		private bool AddStack( Card current ) {
			while ( remains.ContainsKey( current ) == true ) {
				if ( stack.Contains( current ) == true ) {
					return false;
				}
				stack.Push ( current );
				current = remains[current];
			}
			return true;
		}

		private void Init() {
			var piles = board[PileId.Type.Table, PileId.Type.Free];
			remains.Clear();
			foreach ( var pile in piles ) {
				for ( int i=0; i < pile.Count; ++i ) {
					remains.Add( pile.ElementAt( i ), pile.ElementAtOrDefault( i + 1 ) );
				}
			}
		}

		private Card GetLowerRank( Card card ) {
			return new Card( card.suit, card.rank - 1 );
		}

		private class DecendingRank : IComparer<Card> {
			public int Compare( Card left, Card right ) {
				if ( left.rank != right.rank ) {
					return right.rank - left.rank;
				}
				else {
					return right.suit - left.suit;
				}
			}
		}
	}
}