using UnityEngine;
using System.Collections.Generic;

namespace Summoner {

	[System.Serializable]
	public struct Range {
		public float min;
		public float max;

		public Range( float min, float max ) {
			this.min = min;
			this.max = max;
		}
	}

	[System.Serializable]
	public struct RangeInt {
		public int min;
		public int max;

		public RangeInt( int min, int max ) {
			this.min = min;
			this.max = max;
		}

		public bool Contains( int value ) {
			return value >= min
				&& value <= max;
		}
	}
}