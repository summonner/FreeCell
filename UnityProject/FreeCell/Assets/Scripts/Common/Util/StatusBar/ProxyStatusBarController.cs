using UnityEngine;
using System.Collections;

namespace Summoner.Util.StatusBar {
	public class ProxyStatusBarController : IStatusBarController {
		private const float _height = 0.0375f;
		private bool doesShow;

		public void Show( bool show ) {
			doesShow = show;
		}

		public void Dispose() {
			// do nothing
		}

		public int height {
			get {
				if ( doesShow == true ) {
					return Mathf.FloorToInt( Screen.height * _height );
				}
				else {
					return 0;
				}
			}
		}
	}
}
