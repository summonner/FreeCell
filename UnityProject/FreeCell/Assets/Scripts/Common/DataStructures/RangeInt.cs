using UnityEngine;
using System.Collections.Generic;

namespace Summoner {

	///	<summary>Include min value and exclude max value. [min, max) </summary>
	[System.Serializable]
	public struct RangeInt {
		public int min;
		public int max;

		public RangeInt( int min, int max ) {
			this.min = min;
			this.max = max;
		}

		public int Length {
			get {
				return max - min;
			}
		}

		public bool Contains( int value ) {
			return value >= min
				&& value < max;
		}

		public IEnumerator<int> GetEnumerator() {
			return GetEnumerable().GetEnumerator();
		}

		public IEnumerable<int> GetEnumerable() {
			for ( int i = min; i < max; ++i ) {
				yield return i;
			}
		}

		public override string ToString() {
			return "[" + min + ", " + max + ")";
		}
	}
}