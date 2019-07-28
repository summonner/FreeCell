using UnityEngine;
using System.Collections.Generic;

namespace Summoner.Util.Android {
	public class StatusBar : MonoBehaviour {

#if UNITY_ANDROID && !UNITY_EDITOR
		private const int FLAG_LAYOUT_IN_SCREEN		= 0x00000100;
		private const int FLAG_FORCE_NOT_FULLSCREEN = 0x00000800;
		private const int FLAG_TRANSLUCENT_STATUS	= 0x04000000;
		private const int FLAG_DRAWS_SYSTEM_BAR_BACKGROUNDS = unchecked( (int)0x80000000 );
		private const int COLOR_TRANSPARENT			= 0x00000000;

		void Awake() {
			using ( var activity = new MainActivity() ) {
				activity.RunOnUiThread( OnUIThread );
			}
		}

		private void OnUIThread() {
			using ( var activity = new MainActivity() ) {
				var window = activity.GetWindow();
				var windowflag = FLAG_FORCE_NOT_FULLSCREEN
							   | FLAG_LAYOUT_IN_SCREEN
							   | FLAG_DRAWS_SYSTEM_BAR_BACKGROUNDS;
				window.Call( "setFlags", windowflag, -1 );
				window.Call( "setStatusBarColor", COLOR_TRANSPARENT );
			}
		}

		private int GetStatusBarHeight( AndroidJavaObject window ) {
			using ( var deco = window.Call<AndroidJavaObject>( "getDecorView" ) ) {
				using ( var rect = new AndroidJavaObject( "android.graphics.Rect" ) ) {
					deco.Call( "getWindowVisibleDisplayFrame", rect );
					return rect.Get<int>( "top" );
				}
			}
		} 

		private class MainActivity : System.IDisposable {
			private readonly AndroidJavaObject player;
			private readonly AndroidJavaObject activity;
			private AndroidJavaObject window;

			public MainActivity() {
				this.player = new AndroidJavaClass( "com.unity3d.player.UnityPlayer" );
				this.activity = player.GetStatic<AndroidJavaObject>( "currentActivity" );
			}

			public void Dispose() {
				if ( window != null ) {
					window.Dispose();
				}

				activity.Dispose();
				player.Dispose();
			}

			public void RunOnUiThread( System.Action action ) {
				activity.Call( "runOnUiThread", new AndroidJavaRunnable( action ) );
			}

			public AndroidJavaObject GetWindow() {
				return window ?? (window = activity.Call<AndroidJavaObject>( "getWindow" ));
			}
		}
#else
		private static void Trap() {
			var t = UnityEditor.EditorApplication.isPlaying;
		}
#endif
	}
}