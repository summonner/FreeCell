using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Summoner.UI;
using Summoner.Util;
using Summoner.Platform;
using Summoner.Util.Singleton;

namespace Summoner.FreeCell {
	public class CloudSaveController : SingletonBehaviour<CloudSaveController> {
		[SerializeField] private Toggle button;
		[SerializeField] private StageManager stageManager;
		[SerializeField] private LoadingPopup syncingPopup;
		private readonly string loginFailedMessage = "<size=50>Login Failed.</size>";
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
			await UseCloud( useCloud.value, true );
		}

		public async void OnButtonClicked( bool useOffline ) {
			await UseCloud( !useOffline, false );
		}

		private async Task UseCloud( bool enable, bool silent ) {
			using ( new DisableEvent<bool>( button?.onValueChanged, OnButtonClicked ) ) {
				using ( silent ? null : syncingPopup.Show() ) {
					if ( enable == true ) {
						var isSuccess = await Authenticate( silent );
						if ( isSuccess == false ) {
							OnAuthenticateFailed( silent );
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

		private void OnAuthenticateFailed( bool silent ) {
			if ( silent == false ) {
				ToastMessage.Show( loginFailedMessage );
			}

			button.isOn = true;
		}

		private async Task<bool> Authenticate( bool silent ) {
			if ( platform.isAuthenticated == true ) {
				return true;
			}

			return await platform.AuthenticateAsync( silent );
		}
	}
}