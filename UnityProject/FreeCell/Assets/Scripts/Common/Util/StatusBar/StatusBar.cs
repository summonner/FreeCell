using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Summoner.Util.StatusBar {
	public class StatusBar : MonoBehaviour {
		public OnChangeStatusBar onChange;

		private IStatusBarController _nativeModule = null;
		private IStatusBarController nativeModule {
			get {
				return _nativeModule ?? (_nativeModule = Generate());
			}
		}
		private static IStatusBarController Generate() {
	#if UNITY_EDITOR
			return new ProxyStatusBarController();
	#elif UNITY_ANDROID
			return new AndroidStatusBarController();
	#endif
		}

		private int height = 0;

		void OnDestroy() {
			if ( nativeModule != null ) {
				nativeModule.Dispose();
			}
		}

		public void Show( bool enable ) {
			nativeModule.Show( enable );
			if ( enable == true ) {
				StartCoroutine( UpdateHeightDelayed() );
			}
			else {
				UpdateHeight( 0 );
			}
		}

		private IEnumerator UpdateHeightDelayed() {
			yield return null;
			UpdateHeight( nativeModule.height );
		}

		private void UpdateHeight( int newHeight ) {
			var prevHeight = height;
			height = newHeight;
			onChange.Invoke( height, prevHeight );
		}

		[System.Serializable]
		public class OnChangeStatusBar : UnityEvent<int, int> { }
	}
}