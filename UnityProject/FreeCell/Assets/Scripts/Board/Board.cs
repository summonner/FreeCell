using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public interface IBoardLookup {
		IList<Card> Look( PileId id );
	}

	public class Board : IBoardLookup {
		private IList<IPile> homes;
		private IList<IPile> frees;
		private IList<IPile> tables;

		public Board( IBoardLayout layout ) {
			homes = Init<HomeCell>( layout.numHomes );
			frees = Init<FreeCell>( layout.numFrees );
			tables = Init<Tableau>( layout.numPiles );

			InGameEvents.OnClickCard += OnClickCard;
		}

		public Board( IBoardPreset preset ) 
			: this( (IBoardLayout)preset ) 
		{
			Reset( preset.tableau );
			ApplyPreset( homes, preset.homes );
			ApplyPreset( frees, preset.frees );
		}

		private void ApplyPreset( IList<IPile> target, IEnumerable<Card> preset ) {
			int i=0;
			foreach ( var card in preset ) {
				if ( card != Card.Blank ) {
					target[i].Push( card );
				}

				++i;
			}
		}

		IList<Card> IBoardLookup.Look( PileId id ) {
			var piles = GetPiles( id.type );
			if ( piles.IsOutOfRange( id.index ) == true ) {
				return null;
			}

			return piles[id.index].GetReadOnly();
		}

		private IList<IPile> GetPiles( PileId.Type type ) {
			switch ( type ) {
				case PileId.Type.Free:
					return frees;
				case PileId.Type.Home:
					return homes;
				case PileId.Type.Table:
					return tables;
				default:
					return null;
			}
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

		private void OnClickCard( SelectPosition selected ) {
			if ( selected.type == PileId.Type.Home ) {
				return;
			}

			var numMovable = CountMaxMovableCards();
			var selectedPile = GetPiles( selected.type )[selected.column];
			var poped = selectedPile.Pop( selected.row );
			if ( poped == null ) {
				return;
			}

			var target = poped[0];
			foreach ( var nextPileType in SelectNextPile( poped.Count, selected.pile ) ) {
				var piles = GetPiles( nextPileType );
				for ( int i=0; i < piles.Count; ++i ) {
					var next = piles[i];
					if ( next == selectedPile ) {
						continue;
					}

					var IsEmptyTableau = (next is Tableau)
									  && (next.Count == 0);
					var adjustment = IsEmptyTableau ? 2 : 1;
					if ( poped.Count > numMovable / adjustment ) {
						continue;
					}

					if ( next.IsAcceptable( target ) == false ) {
						continue;
					}

					next.Push( poped );
					InGameEvents.MoveCards( poped, new PileId( nextPileType, i ) );
					return;
				}
			}

			selectedPile.Push( poped );
			InGameEvents.CannotMove( poped );
		}

		private IEnumerable<PileId.Type> SelectNextPile( int popedCount, PileId selected ) {
			if ( popedCount == 1 ) {
				yield return PileId.Type.Home;
			}

			yield return PileId.Type.Table;

			if ( popedCount != 1 ) {
				yield break;
			}

			if ( selected.type == PileId.Type.Free ) {
				yield break;
			}

			yield return PileId.Type.Free;
		}

		private int CountMaxMovableCards() {
			var numEmptyFrees =	frees.Count( ( cell ) => (cell.Count == 0) );
			var numEmptyTableau = tables.Count( ( pile ) => (pile.Count == 0) );
			return (1 + numEmptyFrees) * (int)Mathf.Pow( 2f, numEmptyTableau );
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