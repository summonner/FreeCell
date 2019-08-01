using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Summoner.UI;

namespace Summoner.Util.StatusBar {
	public class StatusBar : MonoBehaviour {
		public PresentInt onChange;

		private IStatusBarController nativeModule;
		void Awake() {
	#if UNITY_EDITOR
			nativeModule = new ProxyStatusBarController();
	#elif UNITY_ANDROID
			nativeModule = new AndroidStatusBarController();
	#endif
		}

		public int height {
			get {
				return nativeModule.height;
			}
		}

		void OnDestroy() {
			nativeModule.Dispose();
		}

		void OnEnable() {
			nativeModule.Show( true );
			StartCoroutine( UpdateHeight() );
		}

		void OnDisable() {
			nativeModule.Show( false );
			onChange.Invoke( 0 );
		}

		private IEnumerator UpdateHeight() {
			yield return null;
			onChange.Invoke( height );
		}
	}
}