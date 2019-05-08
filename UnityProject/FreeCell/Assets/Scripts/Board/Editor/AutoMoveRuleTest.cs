using UnityEngine;
using NUnit.Framework;

namespace Summoner.FreeCell.Test {
	public class AutoMoveRuleTest {

		[TestCase( "TableauToFreecell" )]
		[TestCase( "FreecellToTableau" )]
		[TestCase( "TableauToHomecell" )]
		[TestCase( "HomecellToTableau" )]
		[TestCase( "TableauToTableau_single" )]
		[TestCase( "TableauToTableau_multi" )]
		[TestCase( "MoveManyCards" )]
		public void Test( string testCase ) {
			InGameEvents.Flush();

			int numMoves = 0;
			Board board = null;
			TestBoardPreset current = null;
			foreach ( var next in TestBoardPreset.Load( testCase ) ) {
				if ( board == null ) {
					board = new Board( next );
				}
				else {
					InGameEvents.ClickCard( current.select );
				}

				Assert.AreEqual( next, board, "move[" + (numMoves++) + "] failed" );
				current = next;
			}
		}
		
	}
}