using UnityEngine;
using UnityEngine.EventSystems;

namespace Summoner.FreeCell {
	[SelectionBase]
	[RequireComponent( typeof( BoxCollider2D ) )]
	public class CardObject : MonoBehaviour, IPointerDownHandler {
		[SerializeField] private new SpriteRenderer renderer;
		private new Transform transform;
		public System.Action onClick = delegate { };

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
			transform.localPosition = position;
		}

		void Reset() {
			renderer = GetComponentInChildren<SpriteRenderer>();
		}

		public void OnPointerDown( PointerEventData eventData ) {
			Debug.Log( "OnClick " + name );
			onClick();
		}
	}
}