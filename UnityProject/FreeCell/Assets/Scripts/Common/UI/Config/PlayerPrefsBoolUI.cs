using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Summoner.Util;

namespace Summoner.UI.Config {
	public class PlayerPrefsBoolUI : MonoBehaviour {
		[SerializeField] private Toggle button = null;
		[SerializeField] private string preferenceName = "";
		[SerializeField] private bool initialValue = false;
		[SerializeField] private bool inverted = false;
		public PresentToggle onUpdate;

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
			button.isOn = saved.value ^ inverted;
		}

		void Reset() {
			button = GetComponent<Toggle>();
		}

		public void Set( bool enable ) {
			if ( saved == null ) {
				Load();
			}

			enable = enable ^ inverted;
			if ( saved.value != enable ) {
				saved.value = enable;
			}

			onUpdate.Invoke( enable );
		}

		private void Load() {
			saved = PlayerPrefsValue.Bool( preferenceName, initialValue );
		}
	}
}