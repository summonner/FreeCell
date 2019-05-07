using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

using System.Collections;
using System.Collections.Generic;

using Summoner.Util.Extension;

namespace Summoner.FreeCell.Test {
	public class AutoMoveRuleTest {

		public static IEnumerable cases
		{
			get
			{
				// tableau to free cell
				yield return new[] {
					new TestBoardPreset(
						"_ _ _ _", "",
						"SA",
						"H2",
						"C8",
						"*DJ"
					),
					new TestBoardPreset(
						"DJ _ _ _", "",
						"SA",
						"H2",
						"*C8"
					),
					new TestBoardPreset(
						"DJ C8 _ _", "",
						"SA",
						"*H2"
					),
					new TestBoardPreset(
						"DJ C8 H2 _", "",
						"*SA"
					),
					new TestBoardPreset(
						"DJ C8 H2 SA", "",
						""
					)
				};
			}
		}

		[TestCaseSource( "cases" )]
		public void Test( TestBoardPreset[] presets ) {
			var board = new Board( presets[0] );
			for ( int i=1; i < presets.Length; ++i ) {
				var current = presets[i-1];
				var expected = presets[i];
				InGameEvents.ClickCard( current.select );

				try {
					Verify( board, expected.frees, PileId.Type.Free );
					Verify( board, expected.homes, PileId.Type.Home );
					Verify( board, expected.tableau );
				}
				catch ( AssertionException e ) {
					Assert.Fail( "move[" + i + "] failed.\n" + e.Message );
				}
			}
		}

		private static void Verify( IBoardLookup board, IList<Card> cards, PileId.Type type ) {
			for ( int i=0; i < cards.Count; ++i ) {
				var pile = board.Look( new PileId( PileId.Type.Free, i ) );
				var actual = GetLast( pile );
				if ( cards[i] != actual ) {
					throw new AssertionException( type.ToString() + "[" + i + "] missmatched." );
				}
			}
		}

		private static Card GetLast( IList<Card> pile ) {
			if ( pile.Count == 0 ) {
				return Card.Blank;
			}

			return pile[pile.Count - 1];
		}

		private static void Verify( IBoardLookup board, IList<IList<Card>> tableau ) {
			var numPiles = tableau[0].Count;
			for ( int column = 0; column < numPiles; ++column ) {
				var pile = board.Look( new PileId( PileId.Type.Table, column ) );
				for ( int row = 0; row < pile.Count; ++row ) {
					if ( tableau[row][column] != pile[row] ) {
						throw new AssertionException( "Tableau[column : " + column + ", row : " + row + "] missmatched." );
					}
				}
			}
		}
		
	}
}