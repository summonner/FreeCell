using UnityEngine;
using System.Collections.Generic;
using Summoner.Util;

namespace Summoner.FreeCell {
	public class StageSelector {
		private readonly IStageStatesReader stages;
		private readonly ISavedValue<bool> useRandom;
		private readonly ISavedValue<bool> includeCleared;

		public StageSelector( IStageStatesReader stages ) {
			this.stages = stages;
			this.useRandom = PlayerPrefsValue.ReadOnlyBool( "QuickPlay.UseRandom", true );
			this.includeCleared = PlayerPrefsValue.ReadOnlyBool( "QuickPlay.IncludeCleared", true );
		}

		public StageNumber SelectNewStage( SavedStageNumber currentStage ) {
			var randomStage = useRandom.value == true
						   || currentStage == null;
			if ( randomStage == true ) {
				return DrawRandomStage();
			}
			else {
				return DrawNextStage( currentStage );
			}
		}

		public SavedStageNumber GetLastStage() {
			var savedIndex = PlayerPrefsValue.Int( "lastPlayed", -1 );
			if ( savedIndex.value == -1 ) {
				savedIndex.value = DrawRandomStage().index;
			}

			return new SavedStageNumber( savedIndex );
		}

		private StageNumber DrawRandomStage() {
			var randomIndex = -1;
			if ( includeCleared.value == false ) {
				randomIndex = SelectFromNotCleared();
			}

			if ( randomIndex < 0 ) {
				randomIndex = Random.Range( 0, stages.Count );
			}

			return StageNumber.FromIndex( randomIndex );
		}

		private int SelectFromNotCleared() {
			var numNotCleared = stages.Count - stages.numCleared;
			if ( numNotCleared <= 0 ) {
				return -1;
			}

			var notClearedIndex = Random.Range( 0, numNotCleared );
			return stages.IndexOfNotCleared( notClearedIndex );
		}

		private StageNumber DrawNextStage( StageNumber currentStage ) {
			var nextStage = GetNext( currentStage );
			if ( includeCleared.value == true ) {
				return StageNumber.FromIndex( nextStage.index );
			}

			while ( stages.IsCleared( nextStage ) == true ) {
				nextStage = GetNext( nextStage );
			}

			return nextStage;
		}

		private static StageNumber GetNext( StageNumber stageNumber ) {
			var nextIndex = stageNumber.index + 1;
			return StageNumber.FromIndex( nextIndex );
		}
	}
}