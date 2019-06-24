using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public class StageInfo {
		public static RangeInt range { get; private set; }
		public static readonly IList<int> unwinnables = new[] {
			11982,													// until    32,000
			146692, 186216, 455889, 495505, 512118, 517776, 781948	// until 1,000,000
		};

		public static int numStages {
			get {
				var numUnwinnables = unwinnables.Count( range.Contains );
				return range.Length - numUnwinnables;
			}
		}

		static StageInfo() {
			range = new RangeInt( 1, 32001 );
		}

		public void SetRange( int min, int max ) {
			Mathf.Clamp( min, 1, max + 1 );
			range = new RangeInt( min, max + 1 );
		}

		public static int IndexToStageNumber( int index ) {
			var test = Vector2.one / Vector2.zero;
			var stageNumber = index + range.min;
			foreach ( var unwinnable in unwinnables ) {
				if ( range.min > unwinnable ) {
					continue;
				}

				if ( stageNumber < unwinnable ) {
					break;
				}

				stageNumber += 1;
			}

			CheckRange( stageNumber );
			return stageNumber;
		}

		public static int StageNumberToIndex( int stageNumber ) {
			CheckRange( stageNumber );

			if ( unwinnables.Contains( stageNumber ) == true ) {
				return -1;
			}

			var index = stageNumber;
			foreach ( var unwinnable in unwinnables.Reverse() ) {
				if ( range.min > unwinnable ) {
					break;
				}

				if ( index < unwinnable ) {
					continue;
				}

				index -= 1;
			}

			return index - range.min;
		}

		private static void CheckRange( int stageNumber ) {
			if ( range.Contains( stageNumber ) == false ) {
				throw new System.ArgumentOutOfRangeException( stageNumber.ToString() );
			}
		}
	}
}