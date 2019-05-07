using UnityEngine;
using System.Collections.Generic;

namespace Summoner.Util.Extension {
	public static class IListExtension {
		public static bool IsOutOfRange<T>( this IList<T> list, int index ) {
			return list == null
				|| index < 0
				|| index >= list.Count;
		}

		public static System.Collections.ObjectModel.ReadOnlyCollection<T> AsReadOnly<T>( this T[] list ) {
			return System.Array.AsReadOnly( list );
		}
	}
}