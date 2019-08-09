using UnityEngine;
using System.Threading.Tasks;
#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;

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

		public async Task<bool> AuthenticateAsync() {
			TaskCompletionSource<bool> callback = new TaskCompletionSource<bool>();
			Social.localUser.Authenticate( callback.SetResult );
			return await callback.Task;
		}
	}
}
#endif