using UnityEngine;
using System.Collections;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public class MoveAnim : MonoBehaviour {
		private Coroutine anim = null;
		public Vector3 displacement = Vector3.zero;

		[SerializeField] private FloatEffect floater;
		[SerializeField] private AnimationCurve curve = AnimationCurve.Linear( 0f, 0f, 0.1f, 1f );
		public bool isPlaying { get; private set; }

		void Reset() {
			floater = GetComponentInChildren<FloatEffect>();
		}

		public System.Action SetDestination( Vector3 worldPosition, float effectVolume ) {
			isPlaying = true;

			return () => {
				if ( anim != null ) {
					StopCoroutine( anim );
				}
				anim = StartCoroutine( Play( worldPosition, effectVolume ) );
			};
		}

		private IEnumerator Play( Vector3 destination, float effectVolume ) {
			floater.Begin();
			var prevT = 0f;
			foreach ( var t in curve.EvaluateWithTime() ) {
				floater.position += (destination + displacement - floater.position) * (t - prevT) / (1f - prevT);
				yield return null;
				prevT = t;
			}

			if ( effectVolume > 0f ) {
				SoundPlayer.Instance.Play( SoundType.MoveCard, effectVolume );
			}

			if ( displacement == Vector3.zero ) {
				floater.End();
			}

			anim = null;
			isPlaying = false;
		}
	}
}