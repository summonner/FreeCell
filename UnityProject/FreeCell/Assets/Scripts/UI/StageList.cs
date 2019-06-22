using UnityEngine;
using System.Collections.Generic;
using Summoner.UI;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public class StageList : MonoBehaviour {
		[SerializeField] private PresentRatio test = null;
		public int ratio;

		void Awake() {
			GetComponent<DynamicGridLayoutGroup>().numItems = 32000;
			test.Invoke( ratio, 100000 );
		}

		void Update() {
			test.Invoke( ratio, 100000 );
		}

		public void OnInitButton( int index, GameObject target ) {
			var button = target.GetComponent<StageButton>();
			button.Set( index + 1, StageState.Cleared );
		}
	}
}