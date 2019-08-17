using UnityEngine;
using System.Threading.Tasks;
#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Summoner.Platform.Google;

namespace Summoner.Platform {
	public class GooglePlayGamesService : IPlatform {
		[System.Flags]
		public enum Option {
			SavedGame = 0x1,
		}

		public GooglePlayGamesService( Option options ) {
			var config = new PlayGamesClientConfiguration.Builder();
			if ( HasFlags( options, Option.SavedGame ) == true ) {
				config.EnableSavedGames();
			}

			PlayGamesPlatform.InitializeInstance( config.Build() );
			PlayGamesPlatform.Activate();
		}

		public static bool HasFlags( Option option, Option flags ) {
			return (option & flags) == flags;
		}

		public async Task<bool> AuthenticateAsync( bool silent ) {
			TaskCompletionSource<bool> callback = new TaskCompletionSource<bool>();
			PlayGamesPlatform.Instance.Authenticate( callback.SetResult, silent );
			return await callback.Task;
		}

		public bool isAuthenticated => Social.localUser.authenticated;

		public void SignOut() {
			PlayGamesPlatform.Instance.SignOut();
		}

		public Task<ISavedGame> FetchSavedGameAsync( string filename ) {
			return GooglePlayCloudSave.Create( filename );
		}
	}
}
#endif