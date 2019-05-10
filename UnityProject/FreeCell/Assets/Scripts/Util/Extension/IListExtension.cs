using UnityEngine;
using System.Collections.Generic;

namespace Summoner.Util.Extension {
	public static class IListExtension {
		public static bool IsOutOfRange<T>( this IList<T> list, int index ) {
			return list == null
				|| index < 0
				|| index >= list.Count;
		}

		public static bool IsNullOrEmpty<T>( this IList<T> list ) {
			return list == null
				|| list.Count == 0;
		}

		public static int FindIndex<T>( this IList<T> list, System.Predicate<T> match ) {
			var asList = list as List<T>;
			if ( asList != null ) {
				return asList.FindIndex( match );
			}

			if ( list is T[] ) {
				return System.Array.FindIndex( (T[])list, match );
			}

			for ( int i=0; i < list.Count; ++i ) {
				if ( match( list[i] ) == true ) {
					return i;
				}
			}
			return -1;
		}

		public static System.Collections.ObjectModel.ReadOnlyCollection<T> AsReadOnly<T>( this T[] array ) {
			return System.Array.AsReadOnly( array );
		}
	}
}