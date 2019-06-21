using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Summoner.UI {
	[RequireComponent( typeof( Slider ) )]
	public class RatioToSlider : MonoBehaviour {
		public Range range = new Range( 0, 1 );
		private Slider _slider = null;

		private Slider slider {
			get {
				return _slider ?? (_slider = GetComponent<Slider>());
			}
		}

		public void Set( int current, int max ) {
			var ratio = Mathf.Clamp01( (float)current / max );
			var value = range.Lerp( ratio );
			slider.value = value;
		}
	}
}