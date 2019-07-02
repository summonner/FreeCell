using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Summoner.Util;

namespace Summoner.UI.Config {
	public class KeepAwake : MonoBehaviour {
		[SerializeField] private Toggle button;
		[SerializeField] private bool sleepDuringInMenu = true;
		private ISavedValue<bool> saved = null;

#if UNITY_EDITOR
		void OnValidate() {
			if ( button != null ) {
				button.onValueChanged.AddListenerIfNotExist( Set );
			}
		}
#endif

		void Awake() {
			Load();
			SyncButton();
		}

		void OnEnable() {
			if ( sleepDuringInMenu == true ) {
				SetKeepAwake( false );
			}
		}

		void OnDisable() {
			if ( sleepDuringInMenu == true ) {
				SetKeepAwake( saved.value );
			}
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

			if ( sleepDuringInMenu == false ) {
				SetKeepAwake( enable );
			}
		}

		private void SetKeepAwake( bool enable ) {
			Screen.sleepTimeout = enable ? SleepTimeout.NeverSleep : SleepTimeout.SystemSetting;
		}

		private void SyncButton() {
			var isOn = saved.value;
			button.isOn = isOn;
		}
	}
}