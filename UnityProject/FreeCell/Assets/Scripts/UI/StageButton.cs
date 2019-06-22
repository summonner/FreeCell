using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using Summoner.UI;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public enum StageState {
		NotCleared,
		Cleared,
		Unsolvable,
	}

	public class StageButton : MonoBehaviour {
		[SerializeField] private Image symbol;
		[SerializeField] private Sprite[] sprites = null;
		public PresentInt presentStageNumber;

		void Reset() {
			symbol = GetComponentInChildren<Image>();
		}

		public void Set( int stageNumber, StageState _ ) {
			var random = new System.Random( stageNumber );
			var state = (StageState)(random.NextDouble() * 4);
			state = _;

			presentStageNumber.Invoke( stageNumber );
			ShowSymbol( random, state );
		}

		private void ShowSymbol( System.Random random, StageState state ) {
			if ( state == StageState.Cleared ) {
				var index = (int)(random.NextDouble() * sprites.Length);
				var degree = (float)(random.NextDouble() * 360);
				symbol.sprite = sprites.ElementAtOrDefault( index );
				symbol.rectTransform.rotation = Quaternion.Euler( 0, 0, degree );
				symbol.SetNativeSize();
			}
			else {
				symbol.sprite = null;
			}
		}
	}
}