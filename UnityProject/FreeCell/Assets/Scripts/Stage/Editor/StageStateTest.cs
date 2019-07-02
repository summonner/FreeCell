using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;

namespace Summoner.FreeCell.Test {
	[TestFixture]
	public class StageStateTest {
		private static readonly RangeInt range = StageInfo.range;
		public static IEnumerable<StageNumber> GetRandomStages( int numStages ) {
			for ( int i = 0; i < numStages; ++i ) {
				yield return StageNumber.FromIndex( Random.Range( range.min, range.max ) );
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
			using ( var info = new StageStates( new TestData() ) ) {
				var clears = new HashSet<StageNumber>( GetRandomStages( numClear ) );
				foreach ( var clearedStage in clears ) {
					info.Clear( clearedStage );
				}

				Check( info, clears );
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
			var data = new TestData();
			data.map.Add( 0, 0x0000170f );	// 0, 1, 2, 3, 8, 9, 10, 12
			data.map.Add( 7, 0x00c00001 );	// 224, 246, 247
			data.map.Add( 135, -0x10000000 );	// 4351, 4348
			var clears = GetStageNumbers( 0, 1, 2, 3, 8, 9, 10, 12, 224, 246, 247, 4351, 4348 );
			data.numCleared = clears.Length;

			using ( var info = new StageStates( data ) ) {
				Check( info, clears );
			}
		}

		private StageNumber[] GetStageNumbers( params int[] numbers ) {
			return numbers.Select( (number) => ( StageNumber.FromIndex( number ) ) )
						  .ToArray();
		}

		private class TestData : StageStates.IStorageData {
			public readonly IDictionary<int, int> map = new Dictionary<int, int>();
			public int numCleared { get; set; }

			public int Load( int pageIndex ) {
				if ( map.ContainsKey( pageIndex ) == false ) {
					map.Add( pageIndex, 0 );
				}

				return map[pageIndex];
			}

			public void Save( int pageIndex, int values, int numCleared ) {
				map[pageIndex] = values;
				this.numCleared = numCleared;
			}
		}
	}
}