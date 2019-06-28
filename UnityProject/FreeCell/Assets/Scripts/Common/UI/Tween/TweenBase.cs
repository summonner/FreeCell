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

		private RectTransform _rectTransform;
		protected RectTransform rectTransform {
			get {
				if ( _rectTransform == null ) {
					_rectTransform = GetComponent<RectTransform>();
				}

				return _rectTransform;
			}
		}

		public Coroutine Play() {
			return StartCoroutine( Play( curve.EvaluateWithTime() ) );
		}

		public Coroutine PlayReverse() {
			return StartCoroutine( Play( curve.EvaluateWithTimeReverse() ) );
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