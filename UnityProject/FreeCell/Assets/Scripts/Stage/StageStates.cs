using UnityEngine;
using System.Collections.Generic;
using Summoner.Util.Singleton;
using System.Collections.Specialized;

namespace Summoner.FreeCell {
	public interface IStageStatesReader {
		int numCleared { get; }
		int Count { get; }
		bool IsCleared( StageNumber stageNumber );
		int IndexOfNotCleared( int notClearedIndex );
	}

	public class StageStates : Singleton<StageStates>, IStageStatesReader {
		public int numCleared { get; private set; }
		private readonly IStorageData storage;

		public static IStageStatesReader Reader {
			get {
				return instance;
			}
		}

#if UNITY_EDITOR
		public const int defaultSaved = 0;
#endif
		private const int pageSize = 32;
		private IDictionary<int, BitArray> map = null;
		public static readonly int numPages = Mathf.CeilToInt( StageInfo.numStages / (float)pageSize );

		public StageStates() 
			: this( new CloudData() )
		{ }

		public StageStates( IStorageData storage ) {
			if ( storage == null ) {
				throw new System.NullReferenceException( "storage is null" );
			}

			this.storage = storage;
			this.map = new SortedDictionary<int, BitArray>();

			foreach ( var i in new RangeInt( 0, numPages ) ) {
				var saved = storage.Load( i );
				if ( saved != 0 ) {
					map.Add( i, new BitArray( saved ) );
				}
			}

			ClearOutOfRangeValues();
			this.numCleared = CountCleared( map );
		}

		private void ClearOutOfRangeValues() {
			var lastIndex = numPages - 1;
			BitArray lastPage;
			if ( map.TryGetValue( lastIndex, out lastPage ) == false ) {
				return;
			}

			var numOutOfRanges = (pageSize * numPages - StageInfo.numStages);
			var lastPageMask = (int)(uint.MaxValue >> numOutOfRanges);
			map[lastIndex] = new BitArray( lastPage.Data & lastPageMask );
		}

		private static int CountCleared( IDictionary<int, BitArray> map ) {
			var numCleared = 0;
			foreach ( var page in map.Values ) {
				numCleared += page.Count;
			}
			return numCleared;
		}

		public int Count {
			get {
				return StageInfo.numStages;
			}
		}

		public bool IsCleared( StageNumber stageNumber ) {
			var index = new Index( stageNumber );
			if ( map.ContainsKey( index.page ) == false ) {
				return false;
			}
			return map[index.page][index.bitMask];
		}

		public void Clear( StageNumber stageNumber ) {
			if ( IsCleared( stageNumber ) == true ) {
				return;
			}

			var index = new Index( stageNumber );
			if ( map.ContainsKey( index.page ) == false ) {
				Load( index.page );
			}

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
			storage.Save( pageIndex, map[pageIndex].Data );
		}

		public int IndexOfNotCleared( int notClearedIndex ) {
			foreach ( var data in map ) {
				var numNotCleared = pageSize - data.Value.Count;
				if ( notClearedIndex >= numNotCleared ) {
					notClearedIndex -= numNotCleared;
					continue;
				}
				
				foreach ( var i in new RangeInt( 0, pageSize ) ) {
					if ( data.Value[1 << i] == true ) {
						continue;
					}

					notClearedIndex -= 1;
					if ( notClearedIndex < 0 ) {
						return data.Key * pageSize + i;
					}
				}
			}

			return -1;
		}

		private struct BitArray {
			private BitVector32 bits;
			public byte Count { get; private set; }

			public bool this[int bitMask] {
				get {
					return bits[bitMask];
				}
				set {
					bits[bitMask] = value;
					Count = CountOnes( bits );
				}
			}

			public int Data { 
				get {
					return bits.Data;
				}
			}

			public BitArray( int value ) {
				this.bits = new BitVector32( value );
				this.Count = CountOnes( bits );
			}

			private static byte CountOnes( BitVector32 bits ) {
				if ( bits.Data == 0 ) {
					return 0;
				}
				else if ( bits.Data == -1 ) {
					return pageSize;
				}

				byte count = 0;
				foreach ( var i in new RangeInt( 0, pageSize ) ) {
					if ( bits[1 << i] == true ) {
						count += 1;
					}
				}
				return count;
			}
		}

		private struct Index {
			public readonly int page;
			public readonly int bitMask;

			public Index( StageNumber stageNumber ) {
				var stageIndex = stageNumber.index;
				this.page = stageIndex / pageSize;
				this.bitMask = 1 << (stageIndex % pageSize);
			}
		}
	}
}