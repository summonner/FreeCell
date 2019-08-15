using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using Summoner.Util;
using Summoner.Util.DraggableObject;

namespace Summoner.FreeCell {
	public class SlidePopupHandle : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {
		[SerializeField] private SlidePopup target;
		[SerializeField] private bool cannotOpen = false;
		
		private static float finishDuration = 0.1f;
		private static float threshold = 60f;
		private IList<IDraggableObject> dragObjects;
		private Vector3 displacement = Vector3.zero;

		void Reset() {
			target = GetComponent<SlidePopup>();
		}

		public void OnPointerClick( PointerEventData eventData ) {
			if ( eventData.dragging || cannotOpen ) {
				return;
			}

			Open();
		}

		public void OnBeginDrag( PointerEventData eventData ) {
			if ( target == null ) {
				return;
			}

			dragObjects = target.OnBeginDrag();
			displacement = Vector3.zero;
		}

		public void OnDrag( PointerEventData eventData ) {
			if ( dragObjects == null ) {
				return;
			}

			displacement = eventData.GetScreenDisplacement( Vector2.up );
			displacement = AdjustDisplacement( displacement );
			foreach ( var dragObject in dragObjects ) {
				dragObject.OnDrag( displacement );
			}

			if ( displacement.y < -threshold ) {
				Close();
			}
			else if ( displacement.y > threshold ) {
				Open();
			}
		}

		private Vector3 AdjustDisplacement( Vector3 displacement ) {
			if ( target == null ) {
				return displacement;
			}

			if ( displacement.y > 0f ) {
				if ( target.isOpen || cannotOpen ) {
					displacement.y = 0f;
				}
			}
			else if ( displacement.y < 0f ) {
				if ( target.isActiveAndEnabled == false ) {
					displacement.y = 0f;
				}
			}

			return displacement;
		}

		public void OnEndDrag( PointerEventData eventData ) {
			if ( dragObjects == null ) {
				return;
			}

			FinishDrag( displacement );
			displacement = Vector3.zero;
		}

		private void Open() {
			SafeOpenClose( true );
		}

		private void Close() {
			SafeOpenClose( false );
		}

		private void SafeOpenClose( bool open ) {
			if ( target == null ) {
				return;
			}

			if ( open ) {
				target.Open();
			}
			else {
				target.Close();
			}

			dragObjects = null;
		}

		private void FinishDrag( Vector3 lastDisplacement ) {
			if ( dragObjects == null ) {
				return;
			}
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