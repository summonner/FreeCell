using UnityEngine;
using System.Collections.Generic;
using Summoner.Util;

namespace Summoner.FreeCell {
	public class SavedStageNumber {
		private readonly ISavedValue<int> saved;
		public SavedStageNumber( ISavedValue<int> saved ) {
			this.saved = saved;
			this.cached = StageNumber.FromIndex( saved.value );
		}

		private StageNumber cached;
		public StageNumber value {
			get {
				return cached;
			}
			set {
				if ( saved.value == value.index ) {
					return;
				}

				saved.value = value.index;
				cached = value;
			}
		}

		public static implicit operator StageNumber( SavedStageNumber saved ) {
			return saved.value;
		}
	}
}