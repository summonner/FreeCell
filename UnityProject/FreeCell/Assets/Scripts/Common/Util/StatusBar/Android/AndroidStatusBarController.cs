using UnityEngine;
using System.Collections;

namespace Summoner.Util.StatusBar {
#if UNITY_ANDROID
	public class AndroidStatusBarController : IStatusBarController {
		private readonly Controller controller;
		private bool doesShow;

		public AndroidStatusBarController() {
			this.controller = new Controller();
			controller.MakeTransparent();
		}

		public void Dispose() {
			controller.Dispose();
		}

		public void Show( bool show ) {
			doesShow = show;
			controller.Show( show );
		}

		public int height {
			get {
				if ( doesShow == true ) {
					return controller.GetCurrentHeight();
				}
				else {
					return 0;
				}
			}
		}

		private class Controller : System.IDisposable {
			private readonly AndroidJavaClass native;
			public Controller() {
				native = new AndroidJavaClass( "com.Summoner.statusbarcontroller.UnityInterface" );
			}

			public void Dispose() {
				native.Dispose();
			}

			public void MakeTransparent() {
				native.CallStatic( "MakeTransparent" );
			}

			public void Show( bool show ) {
				native.CallStatic( "Show", show );
			}

			public int GetCurrentHeight() {
				return native.CallStatic<int>( "GetCurrentHeight" );
			}

			public int GetHeightFromResource() {
				return native.CallStatic<int>( "GetHeightFromResource" );
			}
		}
	}
#endif
}
