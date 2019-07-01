using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.UI.Tween {
	public abstract class TweenBase : MonoBehaviour {
		public AnimationCurve curve = AnimationCurve.Linear( 0, 0, 1, 1 );

		public float value {
			set {
				SetFrame( value );
			}
		}

		public Coroutine Play() {
			return StartCoroutine( Play( curve.EvaluateWithTime() ) );
		}

		public Coroutine PlayReverse() {
			return StartCoroutine( Play( curve.EvaluateWithTimeReverse() ) );
		}

		public void Play( bool forward ) {
			if ( forward == true ) {
				Play();
			}
			else {
				PlayReverse();
			}
		}

		private IEnumerator Play( IEnumerable<float> iterator ) {
			foreach ( var t in iterator ) {
				SetFrame( t );
				yield return null;
			}
		}

		protected abstract void SetFrame( float t );

		protected void Swap<T>( ref T from, ref T to ) {
			var temp = from;
			from = to;
			to = temp;
		}
	}
}