﻿using UnityEngine;
using NUnit.Framework;

namespace Summoner.FreeCell.Test {
	public class RuleTest {
		[SetUp]
		public void Setup() {
			InGameEvents.Flush();
		}

		[TestCase( "TableauToFreecell" )]
		[TestCase( "FreecellToTableau" )]
		[TestCase( "TableauToHomecell" )]
		[TestCase( "HomecellToTableau" )]
		[TestCase( "TableauToTableau_single" )]
		[TestCase( "TableauToTableau_multi" )]
		[TestCase( "MoveManyCards" )]
		[TestCase( "Traverse" )]
		public void AutoMove( string testCase ) {
			Test( testCase, typeof( AutoPlayToHome ) );
		}
		
		[TestCase( "ClearCheck" )]
		[TestCase( "AutoPlayToHomeAndClear" )]
		public void ClearCheck( string testCase ) {
			int cleared = 0;
			InGameEvents.OnClear += () => { cleared += 1; };
			{
				Test( testCase );
			}
			Assert.AreEqual( 1, cleared, "OnClear event has not occured or too much" );
		}

		[Test]
		public void Undo() {
			var gameObject = new GameObject( "Test.InGameUIEvents" );
			gameObject.AddComponent<InGameUIEvents>();
			{
				Test( "Undo" );
			}
			Object.DestroyImmediate( gameObject );
		}

		private void Test( string testCase, params System.Type[] excludeRules ) {
			int numMoves = 0;
			Board board = null;
			TestBoardPreset current = null;
			foreach ( var next in TestBoardPreset.Load( testCase ) ) {
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
	}
}