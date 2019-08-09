using UnityEngine;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;

namespace Summoner.SavedGame {
	public class GooglePlayCloudSave : ISavedGame {
		private readonly ISavedGameMetadata savedGame;
		private static ISavedGameClient savedGameClient => PlayGamesPlatform.Instance.SavedGame;

		public static async Task<ISavedGame> Create( string filename ) {
			if ( PlayGamesPlatform.Instance.IsAuthenticated() == false ) {
				return null;
			}

			var savedGame = await Open( filename );
			if ( savedGame == null ) {
				return null;
			}

			return new GooglePlayCloudSave( savedGame );
		}

		private static async Task<ISavedGameMetadata> Open( string filename ) {
			var awaiter = new CallbackAwaiter<ISavedGameMetadata>();
			savedGameClient.OpenWithAutomaticConflictResolution(
				filename, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseOriginal, awaiter.Callback );
			return await awaiter;
		}

		private GooglePlayCloudSave( ISavedGameMetadata savedGame ) {
			this.savedGame = savedGame;
		}

		public async Task<byte[]> LoadAsync() {
			var awaiter = new CallbackAwaiter<byte[]>();
			savedGameClient.ReadBinaryData( savedGame, awaiter.Callback );
			return await awaiter;
		}

		public async Task SaveAsync( byte[] data ) {
			var builder = new SavedGameMetadataUpdate.Builder();
			var awaiter = new CallbackAwaiter<ISavedGameMetadata>();
			savedGameClient.CommitUpdate( savedGame, builder.Build(), data, awaiter.Callback );
			await awaiter;
		}

		private class CallbackAwaiter<T> where T : class {
			private readonly TaskCompletionSource<T> tcs;
			public CallbackAwaiter() {
				tcs = new TaskCompletionSource<T>();
			}

			public void Callback( SavedGameRequestStatus status, T result ) {
				if ( status == SavedGameRequestStatus.Success ) {
					tcs.TrySetResult( result );
				}
				else {
					tcs.TrySetResult( result );
				}
			}

			public TaskAwaiter<T> GetAwaiter() {
				return tcs.Task.GetAwaiter();
			}
		}
	}
}
#endif