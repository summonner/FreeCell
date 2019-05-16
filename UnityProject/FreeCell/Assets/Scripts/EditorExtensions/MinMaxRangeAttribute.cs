using UnityEngine;
using System.Collections.Generic;

namespace Summoner {
	public class MinMaxRangeAttribute : PropertyAttribute {
		public readonly float min;
		public readonly float max;

		public MinMaxRangeAttribute( float min, float max ) {
			this.min = min;
			this.max = max;
		}
	}

}