
using UnityEngine;
using System.Collections.Generic;

namespace Summoner.Util.Extension {
	public static class AnimationCurveExtension {
		public static IEnumerable<float> EvaluateWithTime( this AnimationCurve curve ) {
			return EvaluateWithTime( curve, 1f );
		}

		public static IEnumerable<float> EvaluateWithTime( this AnimationCurve curve, float multiplier ) {
			var lastKey = curve[curve.length - 1];
			foreach ( var time in Lerp.Duration( lastKey.time * multiplier ) ) {
				yield return curve.Evaluate( time / multiplier );
			}
		}
	}
}