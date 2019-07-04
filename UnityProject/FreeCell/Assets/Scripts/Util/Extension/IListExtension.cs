using UnityEngine;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace Summoner {
	public static class IListExtension {
		public static bool IsOutOfRange<T>( this ICollection<T> list, int index ) {
			return list == null
				|| index < 0
				|| index >= list.Count;
		}

		public static bool IsNullOrEmpty<T>( this ICollection<T> list ) {
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

		public static IList<T> FindAll<T>( this IList<T> list, System.Predicate<T> match ) {
			var asList = list as List<T>;
			if ( asList != null ) {
				return asList.FindAll( match );
			}

			if ( list is T[] ) {
				return System.Array.FindAll( (T[])list, match );
			}

			return list.Where( new System.Func<T, bool>( match ) )
						.ToArray();
		}

		public static System.Collections.ObjectModel.ReadOnlyCollection<T> AsReadOnly<T>( this T[] array ) {
			return System.Array.AsReadOnly( array );
		}

		public static string Join<T>( this IEnumerable<T> list ) {
			if ( list == null ) {
				return "null";
			}

			StringBuilder str = new StringBuilder();
			str.Append( "[" );
			foreach ( var item in list ) {
				if ( item == null ) {
					str.Append( "null" );
				}
				else {
					str.Append( item.ToString() );
				}
				str.Append( ", " );
			}

			if ( str.Length > 1 ) {
				str.Length -= 2;
			}
			str.Append( "]" );

			return str.ToString();
		}

		public static IEnumerable<KeyValuePair<int, T>> WithIndex<T>( this IEnumerable<T> list ) {
			var i = 0;
			foreach ( var item in list ) {
				yield return new KeyValuePair<int, T>( i++, item );
			}
		}
	}
}