using UnityEngine;
using System.Collections.Generic;

namespace Summoner.Util.Extension {
	public static class IListExtension {
		public static bool IsOutOfRange<T>( this IList<T> target, int index ) {
			return target == null
				|| index < 0
				|| index >= target.Count;
		}
	}
}