using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.UI.Tween {
	public abstract class TweenBase : MonoBehaviour {
		public AnimationCurve curve = AnimationCurve.Linear( 0, 0, 1, 1 );

		public float value {
			set {
				LerpValue( value );
			}
		}

		public Coroutine Play() {
			return StartCoroutine( Play( curve.EvaluateWithTime() ) );
		}

		public Coroutine PlayReverse() {
			return StartCoroutine( Play( curve.EvaluateWithTimeReverse() ) );
		}

		public void Stop() {
			StopAllCoroutines();
			value = 0;
		}

		public void PingPong( bool forward ) {
			if ( forward == true ) {
				Play();
			}
			else {
				PlayReverse();
			}
		}

		public void PlayAndStop( bool play ) {
			if ( play == true ) {
				Play();
			}
			else {
				Stop();
			}
		}

		private IEnumerator Play( IEnumerable<float> iterator ) {
			foreach ( var t in iterator ) {
				LerpValue( t );
				yield return null;
			}
		}

		protected abstract void LerpValue( float t );

		protected void Swap<T>( ref T from, ref T to ) {
			var temp = from;
			from = to;
			to = temp;
		}
	}
}