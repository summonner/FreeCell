using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	[SelectionBase]
	[RequireComponent( typeof( BoxCollider2D ) )]
	public class CardObject : MonoBehaviour, IBoardObject, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {
		[SerializeField] private new SpriteRenderer renderer;
		[SerializeField] private VibrateAnim vibrateAnim;
		[SerializeField] private MoveAnim moveAnim;
		[SerializeField] private FloatEffect floater;

		public PositionOnBoard position { get; set; }

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
			moveAnim = GetComponentInChildren<MoveAnim>();
			floater = GetComponentInChildren<FloatEffect>();
		}

		public void Vibrate() {
			vibrateAnim.StartAnim();
		}

		public System.Action SetDestination( Vector3 worldPosition ) {
			return moveAnim.SetDestination( worldPosition );
		}

		public void OnPointerClick( PointerEventData eventData ) {
			if ( eventData.dragging == true ) {
				return;
			}

			if ( moveAnim.isPlaying == true ) {
				return;
			}

			PlayerInputEvents.Click( position );
		}

		public void OnBeginDrag( PointerEventData eventData ) {
			PlayerInputEvents.BeginDrag( position );
		}

		public void OnDrag( PointerEventData eventData ) {
			var isDraggingOverUi = eventData.enterEventCamera == null;
			if ( isDraggingOverUi ) {
				return;
			}

			var displacement = eventData.pointerCurrentRaycast.worldPosition - eventData.pointerPressRaycast.worldPosition;
			PlayerInputEvents.Drag( position, displacement );
		}

		public void OnEndDrag( PointerEventData eventData ) {
			var receivers = floater.CheckOverlapped();
			if ( receivers.IsNullOrEmpty() == false ) {
				PlayerInputEvents.Drop( position, receivers );
			}

			PlayerInputEvents.EndDrag( position );
		}

		public void BeginFloat() {
			floater.Begin();
		}

		public void Float( Vector3 displacement ) {
			floater.Move( displacement );
		}

		public void EndFloat() {
			var destination = floater.End();
			SetDestination( destination )();
		}
	}
}