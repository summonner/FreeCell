using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Summoner.Util;
using Summoner.Platform;
using Summoner.Util.Singleton;

namespace Summoner.FreeCell {
	public class CloudSaveController : SingletonBehaviour<CloudSaveController> {
		[SerializeField] private Toggle button;
		[SerializeField] private StageManager stageManager;
		[SerializeField] private LoadingPopup syncingPopup;
		private ISavedValue<bool> useCloud = PlayerPrefsValue.Bool( "cloudSave", false );

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
		async void Awake() {
			await UseCloud( useCloud.value );
			InGameEvents.Ready( this );
		}

		public async void OnButtonClicked( bool useOffline ) {
			await UseCloud( !useOffline );
		}

		private async Task UseCloud( bool enable ) {
			using ( new DisableEvent<bool>( button?.onValueChanged, OnButtonClicked ) ) {
				using ( syncingPopup.Show() ) {
					if ( enable == true ) {
						var isSuccess = await Authenticate();
						if ( isSuccess == false ) {
							button.isOn = true;
							return;
						}
					}
					else {
						platform.SignOut();
					}

					button.isOn = !enable;
					useCloud.value = enable;
					await stageManager.RefreshStages();
				}
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