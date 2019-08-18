using UnityEngine;
using System.Collections;
using Summoner.UI;
using Summoner.Util.Singleton;

namespace Summoner.UI {
	public class ToastMessage : SingletonBehaviour<ToastMessage> {
		[SerializeField] private CanvasGroup group = null;
		[SerializeField] private AnimationCurve fadeTime = new AnimationCurve( new Keyframe( 0f, 1f ), new Keyframe( 2f, 1f ), new Keyframe( 3f, 0f, -3f, 3f ) );
		[SerializeField] private PresentString present = null;

		public static void Show( string text ) {
			if ( instance == null ) {
				Debug.LogWarning( "There is no ToastMessage instance" );
				return;
			}
			instance.ShowInternal( text );
		}

		void Reset() {
			this.group = GetComponentInChildren<CanvasGroup>();
		}

		private void ShowInternal( string text ) {
			if ( isActiveAndEnabled == true ) {
				StopAllCoroutines();
			}
			else {
				gameObject.SetActive( true );
			}

			present.Invoke( text );
			StartCoroutine( Fade() );
		}

		private IEnumerator Fade() {
			foreach ( var t in fadeTime.EvaluateWithTime() ) {
				group.alpha = t;
				yield return null;
			}

			gameObject.SetActive( false );
		}
	}
}