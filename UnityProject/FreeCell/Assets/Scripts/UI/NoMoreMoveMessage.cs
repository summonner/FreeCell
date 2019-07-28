using UnityEngine;
using System.Collections.Generic;
using Summoner.UI;

namespace Summoner.FreeCell {
	public class NoMoreMoveMessage : MonoBehaviour {
		public PresentToggle onPlay;

		void OnEnable() {
			InGameEvents.OnNoMoreMoves += Play;
		}

		void OnDisable() {
			InGameEvents.OnNoMoreMoves -= Play;
		}

		private void Play() {
			onPlay.Invoke( true );
			InGameEvents.OnNewGame += Back;
			InGameUIEvents.OnUndo += Back;
			InGameUIEvents.OnReset += Back;
		}

		private void Back() {
			onPlay.Invoke( false );
			InGameEvents.OnNewGame -= Back;
			InGameUIEvents.OnUndo -= Back;
			InGameUIEvents.OnReset -= Back;
		}

		private void Back( StageNumber _ ) {
			Back();
		}
	}
}