using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public class Board {

		public const int numHomeCells = 4;
		private IList<Card?> home;
		public const int numFreeCells = 4;
		private IList<Card?> free;

		public const int numPiles = 8;
		public IList<List<Card>> piles;

		public Board() {
			home = Init<Card?>( numHomeCells );
			free = Init<Card?>( numFreeCells );
			piles = Init<List<Card>>( numPiles );
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