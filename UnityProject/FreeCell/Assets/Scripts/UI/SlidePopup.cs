using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Summoner.Util;
using Summoner.Util.DraggableObject;

namespace Summoner.FreeCell {
	public class SlidePopup : MonoBehaviour {
		[SerializeField] protected PopupAnimController animController;
		[SerializeField] private float offset = -15f;
		protected int key;
		public bool isInTransition { get; private set; }
		public bool isOpen {
			get {
				return animController.IsOpen( key );
			}
		}

		private new RectTransform transform;

		void Reset() {
			this.animController = GetComponentInParent<PopupAnimController>();
		}

		void Awake() {
			transform = GetComponent<RectTransform>();
		}

		public void Init( int key ) {
			this.key = key;
		}

		public void Open() {
			if ( isOpen == true ) {
				return;
			}

			animController.Show( key );
		}

		public void Close() {
			if ( isOpen == true ) {
				animController.Show( key > 0 ? 0 : -1 );
			}
			else {
				animController.Show( key - 1 );
			}
		}

		public void MoveTo( float duration, float y, bool hide ) {
			transform.gameObject.SetActive( true );
			StartCoroutine( MoveToAux( duration, y, hide ) );
		}

		private IEnumerator MoveToAux( float duration, float y, bool hide ) {
			isInTransition = true;
			var startPosition = transform.anchoredPosition;
			var destination = new Vector2( startPosition.x, y + offset );

			foreach ( var elapsed in Lerp.Duration( duration ) ) {
				transform.anchoredPosition = Vector2.Lerp( startPosition, destination, elapsed / duration );
				yield return null;
			}

			if ( hide == true ) {
				transform.gameObject.SetActive( false );
			}
			isInTransition = false;
		}

		public IList<IDraggableObject> OnBeginDrag() {
			return new List<IDraggableObject>( animController.OnBeginDrag( key ) );
		}

		public IDraggableObject GetDraggableObject() {
			return new DraggableRectTransform( transform );
		}
	}
}