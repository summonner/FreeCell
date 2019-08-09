using UnityEngine;
using System.IO;
using System.Collections.Generic;

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

		public byte[] data {
			get {
				if ( File.Exists( filePath ) == false ) {
					return null;
				}

				return File.ReadAllBytes( filePath );
			}

			set {
				File.WriteAllBytes( filePath, value );
			}
		}
	}
}