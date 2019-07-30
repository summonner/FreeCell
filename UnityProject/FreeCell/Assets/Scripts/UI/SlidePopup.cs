using UnityEngine;
using System.Collections;
using Summoner.UI.Popups;
using Summoner.Util;

namespace Summoner.FreeCell {
	public class SlidePopup : BasePopup {
		[SerializeField] protected PopupAnimController animController;
		[SerializeField] private float offset = -15f;
		protected int key;
		public bool isInTransition { get; private set; }

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

		protected override void OnOpen() {
			animController.Show( key );
			SoundPlayer.Instance.Play( SoundType.MenuOpen );
		}

		protected override void OnClose() {
			animController.Show( key > 0 ? 0 : -1 );
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
	}
}