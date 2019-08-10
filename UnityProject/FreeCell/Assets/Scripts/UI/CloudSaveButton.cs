using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Summoner.Util;
using Summoner.Platform;

namespace Summoner.FreeCell {
	public class CloudSaveButton : MonoBehaviour {
		[SerializeField] private Toggle button;
		[SerializeField] private StageManager stageManager;
		[SerializeField] private LoadingPopup syncingPopup;
		private ISavedValue<bool> useCloud = PlayerPrefsValue.Bool( "cloudSave", false );

		private bool invertedValue {
			get {
				return !useCloud.value;
			}

			set {
				var inverted = !value;
				if ( useCloud.value == inverted ) {
					return;
				}

				useCloud.value = inverted;
			}
		}

		private IPlatform platform {
			get {
				return Platform.Instance;
			}
		}

		void Reset() {
			button = GetComponent<Toggle>();
			stageManager = FindObjectOfType<StageManager>();
			syncingPopup = GameObject.Find( "Canvas/Syncing" )?.GetComponent<LoadingPopup>();
		}

#if UNITY_EDITOR
		void OnValidate() {
			button?.onValueChanged.AddListenerIfNotExist( OnButtonClicked );
		}
#endif

		void Awake() {
			button.isOn = invertedValue;
		}

		public async void OnButtonClicked( bool useOffline ) {
			using ( syncingPopup.Show() ) {
				if ( useOffline == true ) {
					platform.SignOut();
				}
				else {
					var isSuccess = await Authenticate();
					if ( isSuccess == false ) {
						button.isOn = true;
						return;
					}
				}

				invertedValue = useOffline;
				await stageManager.RefreshStages();
			}
		}

		private async Task<bool> Authenticate() {
			if ( platform.isAuthenticated == true ) {
				return true;
			}

			return await platform.AuthenticateAsync();
		}
	}
}