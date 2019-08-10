using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using Summoner.Platform.Offline;

namespace Summoner.Platform {
	public class OfflineProxy : IPlatform {
		private readonly int millisecondsDelay;
		private readonly bool allowAuthentication = false;

		public OfflineProxy()
			: this( 0f, true ) { }

		public OfflineProxy( float authenticateDelay, bool allowAuthentication ) {
			this.millisecondsDelay = (int)(authenticateDelay * 1000f);
			this.allowAuthentication = allowAuthentication;
		}

		public bool isAuthenticated { get; private set; }

		public async Task<bool> AuthenticateAsync() {
			await Task.Delay( millisecondsDelay );
			isAuthenticated = allowAuthentication;
			return await Task.FromResult( isAuthenticated );
		}

		public void SignOut() {
			isAuthenticated = false;
		}

		public Task<ISavedGame> FetchSavedGameAsync( string filename ) {
			if ( isAuthenticated == false ) {
				return Task.FromResult<ISavedGame>( null );
			}

			return Task.FromResult<ISavedGame>( new SaveFile( filename, millisecondsDelay ) );
		}
	}
}