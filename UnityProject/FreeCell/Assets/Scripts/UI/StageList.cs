using UnityEngine;
using System.Collections.Generic;
using Summoner.UI;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public class StageList : MonoBehaviour {
		[SerializeField] private PresentRatio test = null;
		private StageStates states;

		void Awake() {
			states = new StageStates();
			GetComponent<DynamicGridLayoutGroup>().numItems = states.Count;
			test.Invoke( states.numCleared, 100000 );
		}

		public void OnInitButton( int index, GameObject target ) {
			var button = target.GetComponent<StageButton>();
			var stageNumber = StageNumber.FromIndex( index );
			button.Set( stageNumber, states[stageNumber] );
		}
	}
}