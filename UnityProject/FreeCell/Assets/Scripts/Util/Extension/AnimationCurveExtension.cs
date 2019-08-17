
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Summoner {
	public static class AnimationCurveExtension {
		public static IEnumerable<float> EvaluateWithTime( this AnimationCurve curve ) {
			return Evaluator.Forward( curve, 1f );
		}

		public static IEnumerable<float> EvaluateWithTime( this AnimationCurve curve, float multiplier ) {
			return Evaluator.Forward( curve, multiplier );
		}

		public static IEnumerable<float> EvaluateWithTimeReverse( this AnimationCurve curve ) {
			return Evaluator.Backward( curve, 1f );
		}

		public static IEnumerable<float> EvaluateWithTimeReverse( this AnimationCurve curve, float multiplier ) {
			return Evaluator.Backward( curve, multiplier );
		}

		private class Evaluator : IEnumerable<float> {
			private readonly AnimationCurve curve;
			private readonly float multiplier;
			private readonly float duration;
			private readonly IEnumerable<float> timeIterator;
			private delegate float AdjustTime( float duration, float current );
			private readonly AdjustTime adjust;

			private Evaluator( AnimationCurve curve, float multiplier, WrapMode wrapMode, AdjustTime adjust ) {
				this.curve = curve;
				this.multiplier = multiplier;
				this.duration = curve[curve.length - 1].time;
				this.adjust = adjust;

				var isLoop = IsLoop( wrapMode );
				if ( isLoop == true ) {
					timeIterator = Lerp.ElapsedTime();
				}
				else {
					timeIterator = Lerp.Duration( duration * multiplier );
				}
			}

			public IEnumerator<float> GetEnumerator() {
				foreach ( var time in timeIterator ) {
					var t = adjust( duration, time / multiplier );
					yield return curve.Evaluate( t );
				}
			}

			IEnumerator IEnumerable.GetEnumerator() {
				return GetEnumerator();
			}

			private static IList<WrapMode> loopModes = new[] { WrapMode.Loop, WrapMode.PingPong };
			private static bool IsLoop( WrapMode wrapMode ) {
				return loopModes.Contains( wrapMode );
			}

			public static Evaluator Forward( AnimationCurve curve, float multiplier ) {
				return new Evaluator( curve, multiplier, curve.postWrapMode, (duration, t) => ( t ) );
			}

			public static Evaluator Backward( AnimationCurve curve, float multiplier ) {
				return new Evaluator( curve, multiplier, curve.preWrapMode, (duration, t) => ( duration - t ) );
			}
		}
	}
}