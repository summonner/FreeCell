using UnityEngine;
using System.Collections.Generic;

namespace Summoner {

	///	<summary>Include min value also max value. [min, max] </summary>
	[System.Serializable]
	public struct Range {
		public float min;
		public float max;

		public Range( float min, float max ) {
			this.min = min;
			this.max = max;
		}

		public float Length {
			get {
				return max - min;
			}
		}

		public bool Contains( int value ) {
			return value >= min
				&& value <= max;
		}

		public float Lerp( float t ) {
			return (1 - t) * min + t * max;
		}

		public float Ratio( float value ) {
			return (value - min) / (max - min);
		}
	}
}