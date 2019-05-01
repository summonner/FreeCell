using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	[SelectionBase]
	[RequireComponent( typeof( BoxCollider2D ) )]
	public class CardObject : MonoBehaviour, IPointerDownHandler {
		[SerializeField] private new SpriteRenderer renderer;
		private new Transform transform;
		private new Collider2D collider;
		public System.Action onClick = delegate { };

		[SerializeField] private AnimationCurve curve = AnimationCurve.Linear( 0f, 0f, 0.1f, 1f );

		public Sprite sprite {
			set {
				if ( renderer == null ) {
					Reset();
				}

				renderer.sprite = value;
			}
		}

		public void SetPosition( Transform pile, Vector3 position ) {
			if ( transform == null ) {
				transform = base.transform;
			}

			transform.parent = pile;
			StopAllCoroutines();
			StartCoroutine( MoveAnim( position ) );
		}

		private IEnumerator MoveAnim( Vector3 destination ) {
			var start = transform.localPosition;
			enableCollider = false;
			renderer.sortingOrder = 1;

			foreach ( var t in curve.EvaluateWithTime() ) {
				transform.localPosition = Vector3.Lerp( start, destination, t );
				yield return null;
			}

			renderer.sortingOrder = 0;
			enableCollider = true;
		}

		private bool enableCollider {
			set {
				if ( collider == null ) {
					collider = GetComponent<Collider2D>();
				}
				collider.enabled = value;
			}
		}

		void Reset() {
			renderer = GetComponentInChildren<SpriteRenderer>();
		}

		public void OnPointerDown( PointerEventData eventData ) {
			onClick();
		}
	}
}