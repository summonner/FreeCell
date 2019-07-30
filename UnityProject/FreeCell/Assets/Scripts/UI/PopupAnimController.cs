using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public class PopupAnimController : MonoBehaviour {
		[SerializeField] private SlidePopup[] popups = null;
		[SerializeField] private int current = -1;
		[SerializeField] private float animDuration = 0.25f;
		[SerializeField] private float spacing = -110f;
		private new RectTransform transform;

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

		public void Show( int key ) {
			current = key;
			Show();
		}

		private void Show() {
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
			else if ( current <= 0 || current >= popups.Length ) {
				return (index - 1) * spacing;
			}
			else {
				hide = true;
				return (index - 1) * spacing - height;
			}
		}
	}
}