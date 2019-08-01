using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Summoner.UI;

namespace Summoner.Util.StatusBar {
	public class StatusBar : MonoBehaviour {
		public PresentInt onChange;

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

		public int height {
			get {
				return nativeModule.height;
			}
		}

		void OnDestroy() {
			if ( nativeModule != null ) {
				nativeModule.Dispose();
			}
		}

		public void Show( bool enable ) {
			nativeModule.Show( enable );
			if ( enable == true ) {
				StartCoroutine( UpdateHeight() );
			}
			else {
				onChange.Invoke( 0 );
			}
		}

		private IEnumerator UpdateHeight() {
			yield return null;
			onChange.Invoke( height );
		}
	}
}