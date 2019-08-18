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
		private DragAnim dragAnim;

		public PositionOnBoard position { get; private set; }

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

		void Awake() {
			dragAnim = new DragAnim( moveAnim, floater );
			moveAnim.target = this;
		}

		public void Vibrate() {
			vibrateAnim.StartAnim();
		}

		public System.Action SetDestination( PositionOnBoard boardPosition, Vector3 worldPosition, float effectVolume ) {
			position = boardPosition;
			dragAnim.startPosition = worldPosition;
			return moveAnim.SetDestination( boardPosition, worldPosition, effectVolume );
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
			PlayerInputEvents.BeginDrag( eventData.pointerId, position );
		}

		public void OnDrag( PointerEventData eventData ) {
			var isDraggingOverUi = eventData.enterEventCamera != Camera.main;
			if ( isDraggingOverUi ) {
				return;
			}

			var displacement = eventData.pointerCurrentRaycast.worldPosition - eventData.pointerPressRaycast.worldPosition;
			PlayerInputEvents.Drag( eventData.pointerId, displacement );
		}

		public void OnEndDrag( PointerEventData eventData ) {
			var receivers = floater.CheckOverlapped();
			if ( receivers.IsNullOrEmpty() == false ) {
				PlayerInputEvents.Drop( eventData.pointerId, position, receivers );
			}

			PlayerInputEvents.EndDrag( eventData.pointerId );
		}

		public void BeginFloat() {
			dragAnim.Begin();
		}

		public void Float( Vector3 displacement ) {
			dragAnim.Move( displacement );
		}

		public void EndFloat( float effectVolume ) {
			dragAnim.End( effectVolume );
		}
	}
}