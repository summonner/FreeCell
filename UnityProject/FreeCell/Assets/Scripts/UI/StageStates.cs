using UnityEngine;
using System.Collections.Generic;
using BitArray = System.Collections.Specialized.BitVector32;

namespace Summoner.FreeCell {
	public class StageStates {
		public static readonly RangeInt range = new RangeInt( 0, 32000 );
		public int numCleared { get; private set; }
		private readonly IStorageData storage;

		private const int pageSize = 32;
		private IDictionary<int, BitArray> map = new Dictionary<int, BitArray>( range.Length / pageSize );

		public static readonly int[] unwinnables = {	// until 1,000,000
			11982, 146692, 186216, 455889, 495505, 512118, 517776, 781948
		};

		public StageStates() 
			: this( new PlayerPrefsData() )
		{ }

		public StageStates( IStorageData storage ) {
			if ( storage == null ) {
				throw new System.NullReferenceException( "storage is null" );
			}

			this.storage = storage;
			numCleared = storage.numCleared;
		}

		public bool this[int stageIndex] {
			get {
				var index = new Index( stageIndex );
				if ( map.ContainsKey( index.page ) == false ) {
					Load( index.page );
				}
				return map[index.page][index.bitMask];
			}
		}

		public void Clear( int stageIndex ) {
			if ( this[stageIndex] == true ) {
				return;
			}

			var index = new Index( stageIndex );
			var page = map[index.page];
			page[index.bitMask] = true;
			map[index.page] = page;
			numCleared += 1;

			Save( index.page );
		}

		private void Load( int pageIndex ) {
			var values = storage.Load( pageIndex );
			var page = new BitArray( values );
			map.Add( pageIndex, page );
		}

		private void Save( int pageIndex ) {
			storage.Save( pageIndex, map[pageIndex].Data, numCleared );
		}



		private struct Index {
			public readonly int page;
			public readonly int bitMask;

			public Index( int stageIndex ) {
				if ( range.IsOutOfRange( stageIndex ) == true ) {
					throw new System.ArgumentOutOfRangeException( "Invalid Stage Number" );
				}

				this.page = stageIndex / pageSize;
				this.bitMask = 1 << stageIndex % pageSize;
			}
		}

		public interface IStorageData {
			int numCleared { get; }
			int Load( int pageIndex );
			void Save( int pageIndex, int values, int numCleared );
		}

		private class PlayerPrefsData : IStorageData {
			private const string numClearedKey = "numCleared";
			public int numCleared {
				get {
					return PlayerPrefs.GetInt( numClearedKey, 0 );
				}
			}

			public int Load( int pageIndex ) {
				return PlayerPrefs.GetInt( ToKey( pageIndex ), 0 );
			}

			public void Save( int pageIndex, int values, int numCleared ) {
				PlayerPrefs.SetInt( ToKey( pageIndex ), values );
				PlayerPrefs.SetInt( numClearedKey, numCleared );
				PlayerPrefs.Save();
			}

			private static string ToKey( int pageIndex ) {
				return pageIndex.ToString();
			}
		}
	}
}