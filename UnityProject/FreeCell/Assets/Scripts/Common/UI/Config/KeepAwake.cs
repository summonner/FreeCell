using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Summoner.Util;

namespace Summoner.UI.Config {
	public class KeepAwake : MonoBehaviour {
		private ISavedValue<bool> saved = null;

		void Awake() {
			Load();
			Set( saved.value );
		}

		private void Load() {
			saved = PlayerPrefsValue.Bool( "keepAwake", false );
		}

		public void Set( bool enable ) {
			if ( saved == null ) {
				Load();
			}

			if ( saved.value != enable ) {
				saved.value = enable;
			}

			Screen.sleepTimeout = enable ? SleepTimeout.NeverSleep : SleepTimeout.SystemSetting;
		}
	}
}