using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public class CheckAllClear : MonoBehaviour {
		[SerializeField] private Toggle includeClearedButton;
		[SerializeField] private Graphic[] graphics;

		void Reset() {
			includeClearedButton = GetComponent<Toggle>();
			graphics = GetComponentsInChildren<Graphic>();
		}

		void OnEnable() {
			var stages = StageStates.Reader;
			if ( stages == null ) {
				return;
			}

			if ( stages.Count <= stages.numCleared ) {
				DisableUIs();
			}
		}

		private void DisableUIs() {
			includeClearedButton.isOn = true;
			includeClearedButton.interactable = false;

			foreach ( var graphic in graphics ) {
				var color = graphic.color;
				color.a = 0.5f;
				graphic.color = color;
			}
		}
	}
}