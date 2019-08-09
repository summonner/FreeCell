using UnityEngine;
using System.IO;
using System.Threading.Tasks;

namespace Summoner.SavedGame {
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

		public SaveFile( string filename ) {
			this.filePath = savePath;
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
			if ( File.Exists( filePath ) == false ) {
				return null;
			}

			return await Task.FromResult( File.ReadAllBytes( filePath ) );
		}
	}
}