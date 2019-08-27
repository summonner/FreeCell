using UnityEngine;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Summoner.FreeCell.Test {
	[TestFixture]
	public class StageStateTest {
		public static IEnumerable<StageNumber> GetRandomStages( int numStages ) {
			for ( int i = 0; i < numStages; ++i ) {
				yield return StageNumber.FromStageNumber( Random.Range( StageInfo.range.min, StageInfo.range.max ) );
			}
		}

		[Test]
		public void Empty() {
			using ( var info = new StageStates( new TestData() ) ) {
				Assert.AreEqual( 0, info.numCleared, "initial numCleared must be 0" );
				foreach ( var stageIndex in GetRandomStages( 5 ) ) {
					Assert.AreEqual( false, info.IsCleared( stageIndex ), "initial state must be false" );
				}
			}
		}

		public static int[] numClear = new[] { 1, 3, 7, 99 };
		[TestCaseSource( "numClear" )]
		public void Clear( int numClear ) {
			using ( new TestStageRange( 1, 100 ) ) {
				using ( var info = new StageStates( new TestData() ) ) {
					var clears = new HashSet<StageNumber>( GetRandomStages( numClear ) );
					foreach ( var clearedStage in clears ) {
						info.Clear( clearedStage );
					}

					Check( info, clears );
				}
			}
		}

		private void Check( StageStates info, ICollection<StageNumber> clears ) {
			Assert.AreEqual( clears.Count, info.numCleared, "numCleared mismatch" );
			var randomStages = clears.Concat( GetRandomStages( 20 ) );
			foreach ( var stage in randomStages ) {
				Assert.AreEqual( clears.Contains( stage ), info.IsCleared( stage ), stage + " stage's state is mismatch" );
			}
		}

		[Test]
		public void InitialData() {
			using ( new TestStageRange( 1, 32000 ) ) {
				var data = new TestData();
				data.map.Add( 0, 0x0000170f );	// 0, 1, 2, 3, 8, 9, 10, 12
				data.map.Add( 7, 0x00c00001 );	// 224, 246, 247
				data.map.Add( 135, unchecked((int)0x90000000) );	// 4351, 4348
				var clears = GetStageNumbers( 0, 1, 2, 3, 8, 9, 10, 12, 224, 246, 247, 4351, 4348 );
				data.numCleared = clears.Length;

				using ( var info = new StageStates( data ) ) {
					info.Refresh().Wait();
					Check( info, clears );
				}
			}
		}

		[TestCase(  0, ExpectedResult = 63 )]
		[TestCase(  1, ExpectedResult = 67 )]
		[TestCase(  2, ExpectedResult = 71 )]
		[TestCase(  3, ExpectedResult = 75 )]
		[TestCase(  4, ExpectedResult = 79 )]
		[TestCase(  5, ExpectedResult = 83 )]
		[TestCase(  6, ExpectedResult = 87 )]
		[TestCase(  7, ExpectedResult = 91 )]
		[TestCase(  8, ExpectedResult = 95 )]
		[TestCase(  9, ExpectedResult = 96 )]
		[TestCase( 10, ExpectedResult = 97 )]
		[TestCase( 11, ExpectedResult = 98 )]
		[TestCase( 12, ExpectedResult = 99 )]
		[TestCase( 13, ExpectedResult = -1 )]
		[TestCase( 14, ExpectedResult = -1 )]
		public int IndexOfNotCleared( int indexOfNotCleared ) {
			using ( new TestStageRange( 1, 100 ) ) {
				var data = new TestData();
				data.map.Add( 0, -1 );
				data.map.Add( 1, 0x7fffffff );
				data.map.Add( 2, 0x77777777 );
				data.numCleared = 32 + 31 + 24;

				using ( var info = new StageStates( data ) ) {
					info.Refresh().Wait();
					return info.IndexOfNotCleared( indexOfNotCleared );
				}
			}
		}

		private StageNumber[] GetStageNumbers( params int[] numbers ) {
			return numbers.Select( (number) => ( StageNumber.FromIndex( number ) ) )
						  .ToArray();
		}

		private class TestData : IStorageData {
			public readonly IDictionary<int, int> map = new Dictionary<int, int>();
			public int numCleared { get; set; }

			public int Load( int pageIndex ) {
				if ( map.ContainsKey( pageIndex ) == false ) {
					return 0;
				}

				return map[pageIndex];
			}

			public void Save( int pageIndex, int values ) {
				map[pageIndex] = values;
			}

			public Task Reimport() {
				// do nothing
				return Task.FromResult<bool>( true );
			}
		}
	}
}