using UnityEngine;
using System.Collections.Generic;
using Summoner.UI.Tween;

namespace Summoner.FreeCell {
	public class NoMoreMoveMessage : MonoBehaviour {
		[SerializeField] private TweenBase tweener;

		void Reset() {
			tweener = GetComponent<TweenBase>();
		}

		void OnEnable() {
			InGameEvents.OnNoMoreMoves += Play;
		}

		void OnDisable() {
			InGameEvents.OnNoMoreMoves -= Play;
		}

		private void Play() {
			tweener.Play();
			InGameEvents.OnNewGame += Back;
			InGameUIEvents.OnUndo += Back;
			InGameUIEvents.OnReset += Back;
		}

		private void Back() {
			tweener.PlayReverse();
			InGameEvents.OnNewGame -= Back;
			InGameUIEvents.OnUndo -= Back;
			InGameUIEvents.OnReset -= Back;
		}

		private void Back( StageNumber _ ) {
			Back();
		}
	}
}