
using UnityEngine;
using System.Collections.Generic;

namespace Summoner.Util.Extension {
	public static class AnimationCurveExtension {
		public static IEnumerable<float> EvaluateWithTime( this AnimationCurve curve ) {
			var lastKey = curve[curve.length - 1];
			foreach ( var time in Lerp.Duration( lastKey.time ) ) {
				yield return curve.Evaluate( time );
			}
		}
	}
}