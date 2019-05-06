using UnityEngine;
using System.Collections;
using Summoner.Util;

namespace Summoner.FreeCell {
	public class VibrateAnim : MonoBehaviour {
		[Range( 0f, 1f )]
		public float duration = 0.1f;
		[Range( 0f, 1f )]
		public float scale = 0.1f;

		private new Transform transform;
		private Vector3 basePosition;

		void Awake() {
			this.transform = base.transform;
			basePosition = transform.localPosition;
		}

		public void StartAnim() {
			StartCoroutine( Anim() );
		}

		private IEnumerator Anim() {
			foreach ( var t in Lerp.Duration( duration ) ) {
				var random = GetRandomVector( t );
				transform.localPosition = basePosition + random;
				yield return null;
			}

			transform.localPosition = basePosition;
		}

		private Vector3 GetRandomVector( float seconds ) {
			if ( seconds >= duration ) {
				return Vector3.zero;
			}
			else {
				return Random.insideUnitCircle * scale;
			}
		}
	}
}