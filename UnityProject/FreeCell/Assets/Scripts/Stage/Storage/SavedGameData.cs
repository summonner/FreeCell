using UnityEngine;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Summoner.Platform;
using Summoner.Util;

namespace Summoner.FreeCell {
	public class SavedGameData : IStorageData {
		private const byte version = 0;
		private const string saveFilename = "freecell.saved";
		private readonly PlayerPrefsData localData;
		private ISavedGame remoteSave;

		public SavedGameData() {
			this.localData = new PlayerPrefsData();
		}

		private static async Task<ISavedGame> FetchRemoteSaveAsync() {
			var useCloud = PlayerPrefsValue.ReadOnlyBool( "cloudSave", false ).value;
			if ( useCloud == true ) {
				return await Platform.Instance.FetchSavedGameAsync( saveFilename );
			}
			else {
				return null;
			}
		}

		public int Load( int pageIndex ) {
			return localData.Load( pageIndex );
		}

		public void Save( int pageIndex, int values ) {
			localData.Save( pageIndex, values );
			Upload();
		}

		public async Task Reimport() {
			remoteSave = await FetchRemoteSaveAsync();
			await DownloadAsync();
			Upload();
		}

		private async Task DownloadAsync() {
			if ( remoteSave == null ) {
				return;
			}

			var serialized = await remoteSave.LoadAsync();
			DeserializeAndMerge( serialized );
		}

		private async void Upload() {
			if ( remoteSave == null ) {
				return;
			}

			await remoteSave.SaveAsync( Serialize() );
		}

		private byte[] Serialize() {
			using ( var stream = new MemoryStream() ) {
				using ( var writer = new BinaryWriter( stream ) ) {
					writer.Write( version );
					foreach ( var i in new RangeInt( 0, StageStates.numPages ) ) {
						var value = localData.Load( i );
						if ( value == 0 ) {
							continue;
						}

						writer.Write( (short)i );
						writer.Write( value );
					}
				}
				return stream.ToArray();
			}
		}

		private void DeserializeAndMerge( byte[] serialized ) {
			if ( serialized.IsNullOrEmpty() == true ) {
				return;
			}

			using ( var parser = Parser.Generate( serialized ) ) {
				foreach ( var page in parser.pages ) {
					var pageIndex = page.Key;
					var cloudValue = page.Value;
					var localValue = localData.Load( pageIndex );
					localData.Save( pageIndex, localValue | cloudValue );
				}
			}
		}

		private abstract class Parser : System.IDisposable {
			private delegate Parser GenerateParser( BinaryReader reader );
			private static readonly IList<GenerateParser> parsers = new GenerateParser[] {
				(reader) => ( new V0( reader ) ),
			};

			public static Parser Generate( byte[] serialized ) {
				var stream = new MemoryStream( serialized );
				var reader = new BinaryReader( stream );
				var version = reader.ReadByte();
				return parsers[version]( reader );
			}

			private readonly Stream stream;
			protected readonly BinaryReader reader;
			protected Parser( BinaryReader reader ) {
				this.reader = reader;
				this.stream = reader.BaseStream;
			}

			public void Dispose() {
				reader.Close();
				stream.Close();
			}

			public IEnumerable<KeyValuePair<int, int>> pages {
				get {
					while ( stream.Position != stream.Length ) {
						var pageIndex = 0;
						var cloudValue = 0;
						GetNext( out pageIndex, out cloudValue );
						yield return new KeyValuePair<int, int>( pageIndex, cloudValue );
					}
				}
			}

			public abstract void GetNext( out int pageIndex, out int value );

			private class V0 : Parser {
				public V0( BinaryReader reader ) : base( reader ) { }

				public override void GetNext( out int pageIndex, out int value ) {
					pageIndex = reader.ReadInt16();
					value = reader.ReadInt32();
				}
			}
		}
	}
}