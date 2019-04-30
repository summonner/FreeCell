using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public class Board {

		private IList<Card?> home;
		private IList<Card?> free;
		public IList<Pile> piles;

		public Board( BoardLayout layout ) {
			home = Init<Card?>( layout.homeCells.Length );
			free = Init<Card?>( layout.freeCells.Length );
			piles = Init<Pile>( layout.piles.Length );
		}

		private static IList<T> Init<T>( int num ) where T : new() {
			var list = new T[num];
			for ( int i=0; i < list.Length; ++i ) {
				list[i] = new T();
			}
			return list;
		}

		public void Reset() {
			Clear();
		}

		public void Clear() {
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
				str.AppendLine( pile.ToString() );
			}
			return str.ToString();
		}
	}
}