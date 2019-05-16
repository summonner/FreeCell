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
}