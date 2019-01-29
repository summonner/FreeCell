using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public class Board {

		private const int numHomeCells = 4;
		private IList<Card?> home;
		private const int numFreeCells = 4;
		private IList<Card?> free;

		private const int numPiles = 8;
		private IList<List<Card>> piles;

		public Board() {
			home = Init<Card?>( numHomeCells );
			free = Init<Card?>( numFreeCells );
			piles = Init<List<Card>>( numPiles );

			Reset();
		}

		private static IList<T> Init<T>( int num ) where T : new() {
			var list = new T[num];
			for ( int i=0; i < list.Length; ++i ) {
				list[i] = new T();
			}
			return list;
		}

		public void Reset() {
			Clear( home );
			Clear( free );
			foreach ( var pile in piles ) {
				pile.Clear();
			}

			var deck = Util.Random.FisherYatesShuffle.Shuffle( Card.NewDeck() );
			for( int i=0; i < deck.Count; ++i ) {
				piles[i % piles.Count].Add( deck[i] );
			}
		}

		private static void Clear( IList<Card?> cells ) {
			for( int i=0; i < cells.Count; ++i ) {
				cells[i] = null;
			}
		}

		public override string ToString() {
			var str = new System.Text.StringBuilder();
			foreach ( var pile in piles ) {
				foreach ( var card in pile ) {
					str.Append( card.ToString() + " " );
				}
				str.AppendLine();
			}
			return str.ToString();
		}
	}
}