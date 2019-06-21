using UnityEngine;
using System.Collections.Generic;
using Summoner.UI;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public class StageList : MonoBehaviour {

		void Awake() {
			GetComponent<DynamicGridLayoutGroup>().numItems = 32000;
		}

		public void OnInitButton( int index, GameObject target ) {
			var button = target.GetComponent<StageButton>();
			button.Set( index, StageState.Cleared );
		}
	}
}