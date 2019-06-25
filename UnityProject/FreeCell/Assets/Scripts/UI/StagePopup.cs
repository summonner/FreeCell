using UnityEngine;
using System.Collections.Generic;
using Summoner.UI;

namespace Summoner.FreeCell {
	public class StagePopup : BasePopup {
		[SerializeField] private DynamicGridLayoutGroup grid;
		[SerializeField] private PresentRatio presentCleared;
		private StageStates stages;

		void Reset() {
			grid = GetComponentInChildren<DynamicGridLayoutGroup>();
		}

		void Awake() {
			stages = new StageStates();
			grid.onInitGridItem.AddListener( OnInitButton );
			grid.numItems = stages.Count;
		}

		void OnDestroy() {
			stages.Dispose();
		}

		public void OnInitButton( int index, GameObject target ) {
			var button = target.GetComponent<StageButton>();
			var stageNumber = StageNumber.FromIndex( index );
			button.Set( stageNumber, stages[stageNumber] );
		}

		public void Show( string stageNumber ) {
			int asInt = 0;
			if ( int.TryParse( stageNumber, out asInt ) == false ) {
				return;
			}

			if ( StageInfo.range.Contains( asInt ) == false ) {
				return;
			}

			var stage = StageNumber.FromStageNumber( asInt );
			grid.Show( stage.index );
		}
	}
}