using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	[SelectionBase]
	[RequireComponent( typeof( BoxCollider2D ) )]
	public class CardObject : MonoBehaviour, IPointerDownHandler {
		[SerializeField] private new SpriteRenderer renderer;
		[SerializeField] private VibrateAnim vibrateAnim;
		[SerializeField] private MoveAnim moveAnim;

		public System.Action onClick = delegate { };

		public Sprite sprite {
			set {
				if ( renderer == null ) {
					Reset();
				}

				renderer.sprite = value;
			}
		}

		void Reset() {
			renderer = GetComponentInChildren<SpriteRenderer>();
			vibrateAnim = GetComponentInChildren<VibrateAnim>();
			moveAnim = GetComponent<MoveAnim>();
		}

		public void OnPointerDown( PointerEventData eventData ) {
			onClick();
		}

		public void Vibrate() {
			vibrateAnim.StartAnim();
		}

		public System.Action SetDestination( Vector3 worldPosition ) {
			return moveAnim.SetDestination( worldPosition );
		}
	}
}