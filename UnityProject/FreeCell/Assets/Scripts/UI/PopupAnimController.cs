using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using System.Collections.Generic;
using Summoner.Util.DraggableObject;
using Summoner.Util.Singleton;

namespace Summoner.FreeCell {
	public class PopupAnimController : SingletonBehaviour<PopupAnimController> {
		[SerializeField] private SlidePopup[] popups = null;
		[SerializeField] private float animDuration = 0.25f;
		[SerializeField] private float spacing = -110f;
		[SerializeField] private float offset = 0f;
		private new RectTransform transform;
		private int current = -1;

		public bool isPlaying {
			get {
				return popups.Any( (popup) => ( popup.isInTransition ) );
			}
		}

		private float height {
			get {
				return transform.rect.height;
			}
		}

		void Awake() {
			transform = GetComponent<RectTransform>();
			
			for ( var i=0; i < popups.Length; ++i ) {
				popups[i].Init( i );
			}
		}

		void Reset() {
			popups = GetComponentsInChildren<SlidePopup>();
		}

		public bool IsOpen( int key ) {
			if ( current == key ) {
				return true;
			}

			return key == 0
				&& current >= 0;
		}

		public void Show( int key ) {
			if ( popups.IsOutOfRange( key ) == true ) {
				current = -1;
			}
			else {
				current = key;
			}

			Show( animDuration );
		}

		public void Ready() {
			Show( 0f );
		}

		private void Show( float animDuration ) {
			if ( animDuration > 0f ) {
				SoundPlayer.Instance.Play( SoundType.MenuOpen );
			}

			var isClose = popups.IsOutOfRange( current );
			var rootPosition = CalculateRootPosition( isClose );
			Play( 0, rootPosition, isClose, animDuration );

			for ( var i=1; i < popups.Length; ++i ) {
				var hide = false;
				var destination = GetDestination( i, out hide );
				Play( i, destination, hide, animDuration );
			}
		}

		private float CalculateRootPosition( bool isClose ) {
			if ( isClose == true ) {
				return -height;
			}
			else {
				return -offset;
			}
		}

		private void Play( int index, float destination, bool hide, float animDuration ) {
			popups[index].MoveTo( animDuration, destination, hide );
		}

		private float GetDestination( int index, out bool hide ) {
			hide = false;
			if ( current == index ) {	// open
				return 0;
			}
			else if ( current <= 0 ) {	// wait
				return (index - 1) * spacing;
			}
			else {
				hide = true;			// close
				return (index - 1) * spacing - height;
			}
		}

		public void CloseLastPopup() {
			if ( current < 0 ) {
				return;
			}
			else if ( current == 0 ) {
				Show( -1 );
			}
			else {
				Show( 0 );
			}
		}

		public IEnumerable<IDraggableObject> OnBeginDrag( int key ) {
			if ( key == 0 ) {
				var root = popups[0];
				yield return root.GetDraggableObject();
				if ( root.gameObject.activeSelf == false ) {
					yield return new ActivatePopup( root );
				}
			}
			else {
				for ( var i = key; i < popups.Length; ++i ) {
					yield return popups[i].GetDraggableObject();
				}
			}
		}

		private class ActivatePopup : IDraggableObject {
			private readonly GameObject gameObject;
			public ActivatePopup( SlidePopup popup ) {
				this.gameObject = popup.gameObject;
			}

			public Vector3 OnDrag( PointerEventData eventData ) {
				throw new System.NotImplementedException();
			}

			public Vector3 OnDrag( PointerEventData eventData, Vector3 mask ) {
				throw new System.NotImplementedException();
			}

			public void OnDrag( Vector3 displacement ) {
				gameObject.SetActive( displacement.y > 0f );
			}
		}
	}
}