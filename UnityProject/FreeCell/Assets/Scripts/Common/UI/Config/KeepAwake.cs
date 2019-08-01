using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Summoner.Util;

namespace Summoner.UI.Config {
	public class KeepAwake : MonoBehaviour {
		[SerializeField] private bool sleepDuringInMenu = true;
		private bool value;

		void OnEnable() {
			if ( sleepDuringInMenu == true ) {
				SetKeepAwake( false );
			}
		}

		void OnDisable() {
			if ( sleepDuringInMenu == true ) {
				SetKeepAwake( value );
			}
		}

		public void Set( bool enable ) {
			value = enable;
			if ( sleepDuringInMenu == false ) {
				SetKeepAwake( enable );
			}
		}

		private void SetKeepAwake( bool enable ) {
			Screen.sleepTimeout = enable ? SleepTimeout.NeverSleep : SleepTimeout.SystemSetting;
		}
	}
}