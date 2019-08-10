using UnityEngine;
using System.IO;
using System.Threading.Tasks;

namespace Summoner.Platform.Offline {
	public class SaveFile : ISavedGame {
		private static string savePath {
			get {
#if UNITY_EDITOR
				return Application.dataPath;
#else
				return Application.persistentDataPath;
#endif
			}
		}

		private readonly string filePath;
		private readonly int millisecondsDelay;

		public SaveFile( string filename )
			: this( filename, 0 ) { }

		public SaveFile( string filename, int loadingDelayMilliseconds ) {
			this.filePath = savePath;
			this.millisecondsDelay = loadingDelayMilliseconds;
			if ( filename.StartsWith( "/" ) == false ) {
				this.filePath += "/";
			}
			
			this.filePath += filename;
		}

		public Task SaveAsync( byte[] data ) {
			File.WriteAllBytes( filePath, data );
			return Task.FromResult( false );
		}

		public async Task<byte[]> LoadAsync() {
			await Task.Delay( millisecondsDelay );

			if ( File.Exists( filePath ) == false ) {
				return null;
			}

			return File.ReadAllBytes( filePath );
		}
	}
}