using UnityEngine;
using NUnit.Framework;

namespace Summoner.FreeCell.Test {
	public class AutoMoveTest {

		[TestCase( "TableauToFreecell" )]
		[TestCase( "FreecellToTableau" )]
		[TestCase( "TableauToHomecell" )]
		[TestCase( "HomecellToTableau" )]
		[TestCase( "TableauToTableau_single" )]
		[TestCase( "TableauToTableau_multi" )]
		[TestCase( "MoveManyCards" )]
		[TestCase( "Traverse" )]
		public void Test( string testCase ) {
			InGameEvents.Flush();

			int numMoves = 0;
			Board board = null;
			TestBoardPreset current = null;
			foreach ( var next in TestBoardPreset.Load( testCase ) ) {
				if ( board == null ) {
					board = new Board( next, typeof( AutoPlayToHome ) );
					board.Reset( next );
				}
				else {
					InGameEvents.ClickCard( current.select );
				}

//				Debug.Log( board );
				Assert.AreEqual( next, board, "move[" + (numMoves++) + "] failed" );
				current = next;
			}
		}
		
	}
}