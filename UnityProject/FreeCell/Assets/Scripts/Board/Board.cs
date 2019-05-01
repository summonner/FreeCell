using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public class Board {

		private IList<IPile> homes;
		private IList<IPile> frees;
		public IList<IPile> tables;

		public Board( BoardLayout layout ) {
			homes = Init<TablePile>( layout.homeCells.Length );
			frees = Init<TablePile>( layout.freeCells.Length );
			tables = Init<TablePile>( layout.piles.Length );

			InGameEvents.OnClickCard += OnClickCard;
		}

		private static IList<IPile> Init<T>( int num ) where T : IPile, new() {
			var list = new IPile[num];
			for ( int i=0; i < list.Length; ++i ) {
				list[i] = new T();
			}
			return list;
		}

		public void Clear() {
			Clear( homes );
			Clear( frees );
			Clear( tables );
		}

		private static void Clear( IList<IPile> piles ) {
			foreach ( var pile in piles ) {
				pile.Clear();
			}
		}

		public void Reset( IEnumerable<Card> cards ) {
			Clear();

			var i = 0;
			foreach ( var card in cards ) {
				var subject = new [] { card };
				var column = i % tables.Count;
				tables[column].Push( subject );

				var destination = new PileId( PileId.Type.Table, column );
				InGameEvents.MoveCards( subject, destination );
				++i;
			}
		}

		private void OnClickCard( PileId pile, int row ) {
			if ( pile.type != PileId.Type.Table ) {
				return;
			}

			var target = tables[pile.index].Peek( row );
			var poped = tables[pile.index].Pop( row );
			var next = (pile.index + 1) % tables.Count;
			tables[next].Push( poped );
			InGameEvents.MoveCards( poped, new PileId( PileId.Type.Table, next ) );
		}



		public override string ToString() {
			var str = new System.Text.StringBuilder();
			foreach ( var pile in tables ) {
				str.AppendLine( pile.ToString() );
			}
			return str.ToString();
		}
	}
}