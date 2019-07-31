using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using Summoner.Util;

namespace Summoner.FreeCell {
	public class SlidePopupHandle : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {
		[SerializeField] private SlidePopup target;
		[SerializeField] private bool ignoreOpen = false;
		
		private static float finishDuration = 0.1f;
		private static float threshold = 50f;
		private IList<IDraggableObject> dragObjects;

		void Reset() {
			target = GetComponent<SlidePopup>();
		}

		public void OnPointerClick( PointerEventData eventData ) {
			if ( eventData.dragging == true ) {
				return;
			}

			Open( target, Vector3.zero );
		}

		public void OnBeginDrag( PointerEventData eventData ) {
			if ( target == null ) {
				return;
			}

			dragObjects = target.OnBeginDrag();
		}

		public void OnDrag( PointerEventData eventData ) {
			if ( dragObjects == null ) {
				return;
			}

			var displacement = eventData.GetScreenDisplacement( Vector2.up );
			foreach ( var dragObject in dragObjects ) {
				dragObject.OnDrag( displacement );
			}

			if ( displacement.y < -threshold ) {
				Close( target );
			}
			else if ( displacement.y > threshold ) {
				Open( target, displacement );
			}
		}

		public void OnEndDrag( PointerEventData eventData ) {
			if ( dragObjects == null ) {
				return;
			}

			FinishDrag( eventData.GetScreenDisplacement( Vector2.up ) );
		}

		private void Open( SlidePopup popup, Vector3 lastDisplacement ) {
			if ( popup == null ) {
				return;
			}

			var donotOpen = ignoreOpen == true
						 || popup.isOpen == true;
			if ( donotOpen == true ) {
				FinishDrag( lastDisplacement );
				return;
			}

			popup.Open();
			dragObjects = null;
		}

		private void Close( SlidePopup popup ) {
			if ( popup == null ) {
				return;
			}

			popup.Close();
			dragObjects = null;
		}

		private void FinishDrag( Vector3 lastDisplacement ) {
			StartCoroutine( FinishDragAux( lastDisplacement ) );
		}

		private IEnumerator FinishDragAux( Vector3 lastDisplacement ) {
			var dragObjects = this.dragObjects;
			this.dragObjects = null;

			foreach ( var elapsed in Lerp.Duration( finishDuration ) ) {
				var displacement = Vector3.Lerp( lastDisplacement, Vector3.zero, elapsed / finishDuration );
				foreach ( var dragObject in dragObjects ) {
					dragObject.OnDrag( displacement );
				}

				yield return null;
			}
		}
	}
}