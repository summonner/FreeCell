using UnityEngine;
using System.Collections;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public class MoveAnim : MonoBehaviour {
		private Coroutine anim = null;

		[SerializeField] private FloatEffect floater;
		[SerializeField] private AnimationCurve curve = AnimationCurve.Linear( 0f, 0f, 0.1f, 1f );
		public bool isPlaying { get; private set; }

		void Reset() {
			floater = GetComponentInChildren<FloatEffect>();
		}

		public System.Action SetDestination( Vector3 worldPosition ) {
			isPlaying = true;

			return () => {
				if ( anim != null ) {
					StopCoroutine( anim );
				}
				anim = StartCoroutine( Play( worldPosition ) );
			};
		}

		private IEnumerator Play( Vector3 destination ) {
			var start = floater.Begin();

			foreach ( var t in curve.EvaluateWithTime() ) {
				floater.Move( (destination - start) * t );
				yield return null;
			}

			floater.End();
			anim = null;
			isPlaying = false;
		}
	}
}