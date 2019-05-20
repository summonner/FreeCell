using UnityEngine;

namespace Summoner {
	public class MinMaxRangeAttribute : PropertyAttribute {
		public readonly float min;
		public readonly float max;

		public readonly string minProperty;
		public readonly string maxProperty;

		public MinMaxRangeAttribute( float min, float max )
			: this( min, max, "min", "max" ) { }

		public MinMaxRangeAttribute( float min, float max, string minProperty, string maxProperty ) {
			this.min = min;
			this.max = max;
			this.minProperty = minProperty;
			this.maxProperty = maxProperty;
		}
	}

}