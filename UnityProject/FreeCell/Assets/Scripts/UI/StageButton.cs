using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using Summoner.UI;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	[SelectionBase]
	public class StageButton : MonoBehaviour {
		[SerializeField] private Image symbol;
		[SerializeField] private Sprite[] sprites = null;
		public PresentInt presentStageNumber;
		public int debug;

		void Reset() {
			symbol = GetComponentInChildren<Image>();
		}

		public void Set( StageNumber stageNumber, bool isCleared ) {
			debug = stageNumber.GetHashCode();	
			var random = new System.Random( stageNumber.GetHashCode() );
			var state = true;
			
			presentStageNumber.Invoke( stageNumber );
			ShowSymbol( random, state );
		}

		private void ShowSymbol( System.Random random, bool isCleared ) {
			if ( isCleared == true ) {
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