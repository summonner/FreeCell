
using UnityEngine;
using System.Collections.Generic;

namespace Summoner.Util.Extension {
	public static class AnimationCurveExtension {
		public static IEnumerable<float> EvaluateWithTime( this AnimationCurve curve ) {
			return EvaluateWithTime( curve, 1f );
		}

		public static IEnumerable<float> EvaluateWithTime( this AnimationCurve curve, float multiplier ) {
			return EvaluateWithTime( curve, multiplier, (duration, t) => ( t ) );
		}

		public static IEnumerable<float> EvaluateWithTimeReverse( this AnimationCurve curve ) {
			return EvaluateWithTimeReverse( curve, 1f );
		}

		public static IEnumerable<float> EvaluateWithTimeReverse( this AnimationCurve curve, float multiplier ) {
			return EvaluateWithTime( curve, multiplier, (duration, t) => ( duration - t ) );
		}

		private delegate float AdjustTime( float duration, float current );
		private static IEnumerable<float> EvaluateWithTime( AnimationCurve curve, float multiplier, AdjustTime timeFunc ) {
			var duration = curve[curve.length - 1].time;
			foreach ( var time in Lerp.Duration( duration * multiplier ) ) {
				yield return curve.Evaluate( timeFunc( duration, time / multiplier ) );
			}
		}
	}
}