using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.FreeCell.Test {
	public class StageNumberTest {
		private static IEnumerable<int[]> testCases {
			get {
				yield return new[] {      0,       1 };
				yield return new[] {      1,       2 };
				yield return new[] {  10000,   10001 };
				yield return new[] {  31998,   32000 };
				yield return new[] { 999991, 1000000 };
			}
		}

		private static IEnumerable<int[]> nearUnwinnables {
			get {
				var unwinnables = StageInfo.unwinnables;
				var diffs = new [] { 1, 2 };
				for ( int i=0; i < unwinnables.Count; ++i ) {
					foreach ( var diff in diffs ) {
						var stageNumber = unwinnables[i] - diff;
						yield return new [] { stageNumber - 1 - i, stageNumber };

						stageNumber = unwinnables[i] + diff;
						yield return new [] { stageNumber - 1 - (i + 1), stageNumber };
					}
				}
			}
		}

		private static IEnumerable<int[]> unwinnables {
			get {
				foreach ( var unwinnable in StageInfo.unwinnables ) {
					yield return new [] { -1, unwinnable };
				}
			}
		}

		private class TestStageRange : System.IDisposable {
			private readonly StageInfo controller = new StageInfo();
			public TestStageRange()
				: this( 1, 1000000 ) { }

			public TestStageRange( int min, int max ) {
				StageInfo.SetRange( min, max );
			}

			public void Dispose() {
				StageInfo.SetRange( 1, 32000 );
			}
		}

		[TestCaseSource( "testCases" )]
		[TestCaseSource( "nearUnwinnables" )]
		public void FromIndex( int index, int value ) {
			using ( new TestStageRange() ) {
				var stage = StageNumber.FromIndex( index );
				Check( index, value, stage );
			}
		}

		[TestCaseSource( "testCases" )]
		[TestCaseSource( "nearUnwinnables" )]
		[TestCaseSource( "unwinnables" )]
		public void FromStageNumber( int index, int value ) {
			using ( new TestStageRange() ) {
				var stage = StageNumber.FromStageNumber( value );
				Check( index, value, stage );
			}
		}

		private void Check( int index, int value, StageNumber actual ) {
			Check( index, value, actual, null );
		}

		private void Check( int index, int value, StageNumber actual, string text ) {
			Assert.AreEqual( index, actual.index, text + "Indices are mismatched" );
			Assert.AreEqual( value, actual.value, text + "Stage numbers are mismatched" );
		}

		[TestCase( -1 )]
		[TestCase( 0 )]
		[TestCase( 32001 )]
		public void OutOfRangedStageNumber( int stageNumber ) {
			CheckException( () => { StageNumber.FromStageNumber( stageNumber ); } );
		}

		[TestCase( -1 )]
		[TestCase( 32000 )]
		public void OutOfRangedIndex( int index ) {
			CheckException( () => { StageNumber.FromIndex( index ); } );
		}

		private void CheckException( TestDelegate action ) {
			Assert.Throws( typeof( System.ArgumentOutOfRangeException ), action );
		}

		[TestCase( 0, 100000 )]
		[TestCase( 355887, 455890 )]
		[TestCase( 899993, 1000000 )]
		public void RangeTest( int index, int value ) {
			using ( new TestStageRange( 100000, 1000000 ) ) {
				Assert.AreEqual( 1000000 - 100000 - 7 + 1, StageInfo.numStages, "numStages are mismatched" );
				Check( index, value, StageNumber.FromIndex( index ), "FromIndex : " );
				Check( index, value, StageNumber.FromStageNumber( value ), "FromValue : " );
			}
		}
	}
}