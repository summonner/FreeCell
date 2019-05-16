using UnityEngine;
using System.Collections;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public class MoveAnim : MonoBehaviour {
		private new Transform transform;
		private new Collider2D collider;
		private Coroutine anim = null;

		[SerializeField] private new SpriteRenderer renderer;
		[SerializeField] private AnimationCurve curve = AnimationCurve.Linear( 0f, 0f, 0.1f, 1f );

		void Reset() {
			renderer = GetComponentInChildren<SpriteRenderer>();
		}

		public System.Action SetDestination( Vector3 worldPosition ) {
			if ( transform == null ) {
				transform = base.transform;
			}
			enableCollider = false;

			return () => {
				if ( anim != null ) {
					StopCoroutine( anim );
				}
				anim = StartCoroutine( Play( worldPosition ) );
			};
		}

		private IEnumerator Play( Vector3 destination ) {
			var start = transform.position;
			enableCollider = false;
			renderer.sortingOrder = 1;

			foreach ( var t in curve.EvaluateWithTime() ) {
				transform.position = Vector3.Lerp( start, destination, t );
				yield return null;
			}

			renderer.sortingOrder = 0;
			enableCollider = true;
			anim = null;
		}

		private bool enableCollider
		{
			set
			{
				if ( collider == null ) {
					collider = GetComponent<Collider2D>();
				}
				collider.enabled = value;
			}
		}
	}
}