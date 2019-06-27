using UnityEngine;
using System.Collections;
using Summoner.Util.Extension;

namespace Summoner.UI.Tween {
	[RequireComponent( typeof(RectTransform) )]
	public class TweenScale : MonoBehaviour {
		public Vector2 from;
		public Vector2 to;
		public AnimationCurve curve = AnimationCurve.Linear( 0, 0, 1, 1 );

		private RectTransform _rectTransform;
		private RectTransform rectTransform {
			get {
				if ( _rectTransform == null ) {
					_rectTransform = GetComponent<RectTransform>();
				}

				return _rectTransform;
			}
		}

		public float value {
			set {
				SetFrame( value );
			}
		}

		public Coroutine Play() {
			return StartCoroutine( PlayInternal() );
		}

		private IEnumerator PlayInternal() {
			foreach ( var t in curve.EvaluateWithTime() ) {
				SetFrame( t );
				yield return null;
			}
		}

		private void SetFrame( float t ) {
			rectTransform.localScale = Vector2.Lerp( from, to, t );
		}
	}
}