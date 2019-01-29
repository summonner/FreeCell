using System.Collections.Generic;

namespace Summoner.Util.Random {
	
	public static class FisherYatesShuffle {
		public delegate int RandomFunc( int min, int max );

		public static IEnumerable<T> Draw<T>( IList<T> source ) {
			return Draw( source, UnityEngine.Random.Range );
		}

		public static IEnumerable<T> Draw<T>( IList<T> source, RandomFunc random ) {
			var indexes = CreateIndexes( source.Count );
			for ( var i = indexes.Count; i > 0; --i ) {
				var selected = random( 0, i );
				var index = indexes[selected];
				yield return source[index];
				indexes[selected] = indexes[i - 1];
			}
		}

		private static IList<int> CreateIndexes( int maxIndex ) {
			var indexes = new int[maxIndex];
			for ( int i=0; i < maxIndex; ++i ) {
				indexes[i] = i;
			}
			return indexes;
		}

		public static List<T> Shuffle<T>( IEnumerable<T> source ) {
			return Shuffle( source, UnityEngine.Random.Range );
		}

		public static List<T> Shuffle<T>( IEnumerable<T> source, RandomFunc random ) {
			var shuffled = new List<T>();
			foreach ( var item in source ) {
				var selected = random( 0, shuffled.Count );
				shuffled.Insert( selected, item );
			}
			return shuffled;
		}
	}
}
