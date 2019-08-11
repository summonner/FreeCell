#if UNITY_EDITOR
using UnityEditor.Events;
#endif
using UnityEngine;
using UnityEngine.UI;

namespace Summoner.Sound {
	public class VolumeControlUI : MonoBehaviour {
		[SerializeField] private Slider slider = null;
		[SerializeField] private Toggle button = null;
		[SerializeField] private VolumeController controller = null;

#if UNITY_EDITOR
		void Reset() {
			slider = GetComponentInChildren<Slider>();
			button = GetComponentInChildren<Toggle>();
			AddListeners();
		}

		[ContextMenu("AddListeners")]
		private void AddListeners() {
			if ( slider != null ) {
				slider.onValueChanged.AddListenerIfNotExist( Set );
			}

			if ( button != null ) {
				button.onValueChanged.AddListenerIfNotExist( Set );
			}
		}
#endif
		void Start() {
			controller.Load();
			SyncButton();
			SyncSlider();
		}

		public void Set( float normalizedVolume ) {
			controller.normalizedVolume = normalizedVolume;
			SyncButton();
		}

		public void Set( bool enable ) {
			controller.Mute( enable == false );
			SyncSlider();
		}

		private void SyncButton() {
			if ( button == null ) {
				return;
			}

			var isOn = controller.isMuted == false;
			if ( button.isOn == isOn ) {
				return;
			}

			using ( new DisableEvent<bool>( button.onValueChanged, Set ) ) {
				button.isOn = isOn;
			}
		}

		private void SyncSlider() {
			if ( slider == null ) {
				return;
			}

			var value = controller.normalizedVolume;
			if ( slider.value == value ) {
				return;
			}

			using ( new DisableEvent<float>( slider.onValueChanged, Set ) ) {
				slider.value = value;
			}
		}
	}
}