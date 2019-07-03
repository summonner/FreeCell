using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Summoner.Util;

namespace Summoner.UI {
	public class ToggleForPlayerPrefsValue : MonoBehaviour {
		[SerializeField] private Toggle button = null;
		[SerializeField] private string preferenceName = null;
		private ISavedValue<bool> _saved;
		private ISavedValue<bool> saved {
			get {
				return _saved ?? (_saved = PlayerPrefsValue.Bool( preferenceName, button.isOn ));
			}
		}

#if UNITY_EDITOR
		void Reset() {
			button = GetComponent<Toggle>();
		}

		void OnValidate() {
			if ( button != null ) {
				button.onValueChanged.AddListenerIfNotExist( Set );
			}
		}
#endif
		void OnEnable() {
			Sync();
		}

		private void Sync() {
			button.isOn = saved.value;
		}

		private void Set( bool enabled ) {
			if ( enabled == saved.value ) {
				return;
			}

			saved.value = enabled;
		}
	}
}