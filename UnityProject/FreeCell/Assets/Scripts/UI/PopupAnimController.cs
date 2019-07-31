using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Summoner.Util;
using Summoner.Util.Singleton;

namespace Summoner.FreeCell {
	public class PopupAnimController : SingletonBehaviour<PopupAnimController> {
		[SerializeField] private SlidePopup[] popups = null;
		[SerializeField] private float animDuration = 0.25f;
		[SerializeField] private float spacing = -110f;
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

			Show();
		}

		private void Show() {
			SoundPlayer.Instance.Play( SoundType.MenuOpen );

			var isClose = popups.IsOutOfRange( current );
			Play( 0, height * (isClose ? -1f : 0f), isClose );

			for ( var i=1; i < popups.Length; ++i ) {
				var hide = false;
				var destination = GetDestination( i, out hide );
				Play( i, destination, hide );
			}
		}

		private void Play( int index, float destination, bool hide ) {
			popups[index].MoveTo( animDuration, destination, hide );
		}

		private float GetDestination( int index, out bool hide ) {
			hide = false;
			if ( current == index ) {
				return 0;
			}
			else if ( current <= 0 ) {
				return (index - 1) * spacing;
			}
			else {
				hide = true;
				return (index - 1) * spacing - height;
			}
		}

		public IEnumerable<IDraggableObject> OnBeginDrag( int key ) {
			if ( key == 0 ) {
				yield return popups[0].GetDraggableObject();
			}
			else {
				for ( var i=key; i < popups.Length; ++i ) {
					yield return popups[i].GetDraggableObject();
				}
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

		void Update() {
			if ( Input.GetKeyDown( KeyCode.Escape ) == true ) {
				CloseLastPopup();
			}
		}
	}
}