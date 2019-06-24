using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public struct StageNumber {
		public readonly int value;
		public readonly int index;

		private StageNumber( int value, int index ) {
			this.value = value;
			this.index = index;
		}

		public static StageNumber FromIndex( int index ) {
			var value = StageInfo.IndexToStageNumber( index );
			return new StageNumber( value, index );
		}

		public static StageNumber FromStageNumber( int stageNumber ) {
			var index = StageInfo.StageNumberToIndex( stageNumber );
			return new StageNumber( stageNumber, index );
		}

		public static implicit operator int( StageNumber number ) {
			return number.value;
		}
	}
}