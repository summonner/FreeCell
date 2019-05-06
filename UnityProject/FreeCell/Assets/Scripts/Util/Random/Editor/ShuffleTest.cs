using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;

namespace Summoner.Util.Random.Test {
	public class ShuffleTest {

		public static int[] testCases = new [] { 1, 10, 100 };

		[TestCaseSource("testCases")]
		public void FisherYate_Draw( int num ) {
			var sample = new List<int>( CreateSample( num ) );
			var shuffled = new List<int>( FisherYatesShuffle.Draw( sample ) );

			var sourceValidationError = "Source data has currupted";
			Assert.AreEqual( num, sample.Count, sourceValidationError );
			for ( int i=0; i < sample.Count; ++i ) {
				Assert.AreEqual( i, sample[i], sourceValidationError );
			}

			ValidateWithSource( sample, shuffled );

			var shuffled2 = new List<int>( FisherYatesShuffle.Draw( sample ) );
			ValidateWithOther( shuffled, shuffled2 );
		}

		private static IEnumerable<int> CreateSample( int num ) {
			for ( int i=0; i < num; ++i ) {
				yield return i;
			}
		}

		private static void ValidateWithSource( List<int> source, List<int> shuffled ) {
			var sample = new List<int>( source );
				
			Assert.AreEqual( sample.Count, shuffled.Count, "Item count mismatch" );
			foreach ( var i in shuffled ) {
				var removed = sample.Remove( i );
				Assert.IsTrue( removed, "Duplicated shuffle data" );
			}

			Assert.AreEqual( 0, sample.Count, "Incompleted shuffle data" );
		}

		private static bool ValidateWithOther( List<int> first, List<int> second ) {
			if ( first.Count <= 1 ) {
				return true;
			}

			Assert.AreEqual( first.Count, second.Count, "Item count mismatch" );
			for ( int i = 0; i < first.Count; ++i ) {
				if ( second[i] != first[i] ) {
					return true;
				}
			}

			Assert.Fail( "Same result between two" );
			return false;
		}

		[TestCaseSource("testCases")]
		public void FisherYate_Shuffle( int num ) {
			var source = CreateSample( num );
			var sample = new List<int>( source );
			var shuffled = FisherYatesShuffle.Shuffle( source );

			ValidateWithSource( sample, shuffled );

			var shuffled2 = FisherYatesShuffle.Shuffle( source );
			ValidateWithOther( shuffled, shuffled2 );
		}
	}
}