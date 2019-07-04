using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;

namespace Summoner.FreeCell.Test {
	public class RuleTest {
		[SetUp]
		public void Setup() {
			InGameEvents.Flush();
			PlayerInputEvents.Flush();
		}

		[TestCase( "TableauToFreecell" )]
		[TestCase( "FreecellToTableau" )]
		[TestCase( "TableauToHomecell" )]
		[TestCase( "HomecellToTableau" )]
		[TestCase( "TableauToTableau_single" )]
		[TestCase( "TableauToTableau_multi" )]
		[TestCase( "MoveManyCards" )]
		[TestCase( "Traverse" )]
		[TestCase( "DragAndDrop" )]
		public void AutoMove( string testCase ) {
			Test( testCase, typeof( AutoPlayToHome ) );
		}
		
		[TestCase( "ClearCheck" )]
		[TestCase( "AutoPlayToHomeAndClear" )]
		public void ClearCheck( string testCase ) {
			int cleared = 0;
			InGameEvents.OnGameClear += () => { cleared += 1; };
			using ( new AutoPlayScheduler() ) {
				Test( testCase );
			}
			Assert.AreEqual( 1, cleared, "OnClear event has not occured or too much" );
		}

		[TestCase( "Undo" )]
		[TestCase( "UndoWithDragAndDrop" )]
		public void Undo( string testCase ) {
			var gameObject = new GameObject( "Test.InGameUIEvents" );
			gameObject.AddComponent<InGameUIEvents>();
			using ( new AutoPlayScheduler() ) {
				Test( testCase );
			}
			Object.DestroyImmediate( gameObject );
		}

		[TestCase( "NoMoreMoves" )]
		[TestCase( "NoMoreMoves_simple" )]
		public void NoMoreMoves( string testCase ) {
			int noMoreMoves = 0;
			InGameEvents.OnNoMoreMoves += () => { noMoreMoves += 1; };
			using ( new AutoPlayScheduler() ) {
				Test( testCase );
			}
			Assert.AreEqual( 1, noMoreMoves, "OnNoMoreMoves event has not occured or too much" );
		}

		private void Test( string testCase, params System.Type[] excludeRules ) {
			int numMoves = 0;
			Board board = null;
			TestBoardPreset current = null;
			foreach ( var next in TestCaseParser.Load( testCase ) ) {
				if ( board == null ) {
					board = new Board( next, excludeRules );
					board.Reset( next );
				}
				else {
					current.ApplyOperation();
				}

//				Debug.Log( board );
				Assert.AreEqual( next, board, "move[" + (numMoves++) + "] failed" );
				current = next;
			}
		}

		private class AutoPlayScheduler : System.IDisposable {
			public AutoPlayScheduler() {
				InGameEvents.OnPlayerMove += CheckAutoPlay;
				InGameEvents.OnAutoPlay += CheckAutoPlay;
			}

			public void Dispose() {
				InGameEvents.OnPlayerMove -= CheckAutoPlay;
				InGameEvents.OnAutoPlay -= CheckAutoPlay;
			}

			private void CheckAutoPlay( IEnumerable<Card> _1, PileId _2, PileId _3 ) {
				InGameEvents.CheckAutoPlay();
			}
		}
	}
}