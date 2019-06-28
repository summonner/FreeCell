using UnityEngine;
using System.Collections;
using Summoner.UI;

namespace Summoner.FreeCell {
	public class StageSelector : MonoBehaviour {
		[SerializeField] private int testSeed = -1;
		[SerializeField] private StagePopup popup = null;
		[SerializeField] private PresentInt onChangeSeed = null;

		private StageStates stages = null;
		private StageNumber currentStage;

		void Awake() {
			stages = new StageStates();
			popup.Init( stages );

			InGameEvents.OnNewGame += OnNewGame;
			InGameEvents.OnGameClear += OnClear;
			InGameUIEvents.OnCloseTitle += PlayRandomGame;
			InGameUIEvents.OnQuickGame += PlayRandomGame;
		}

		void OnDestroy() {
			stages.Dispose();

			InGameEvents.OnNewGame -= OnNewGame;
			InGameEvents.OnGameClear -= OnClear;
			InGameUIEvents.OnCloseTitle -= PlayRandomGame;
			InGameUIEvents.OnQuickGame -= PlayRandomGame;
		}

		private void OnNewGame( StageNumber stageNumber ) {
			currentStage = stageNumber;
			onChangeSeed.Invoke( stageNumber );
			popup.SetScroll( stageNumber );
		}

		public void PlayRandomGame() {
			var randomStage = DrawRandomStage();
			InGameEvents.NewGame( randomStage );
		}

		private StageNumber DrawRandomStage() {
#if UNITY_EDITOR
			if ( testSeed > 0 ) {
				return StageNumber.FromStageNumber( testSeed );
			}
#endif
			var randomIndex = Random.Range( 0, StageInfo.numStages );
			return StageNumber.FromIndex( randomIndex );
		}

		private void OnClear() {
			StartCoroutine( PlayClearAnim() );
		}

		private IEnumerator PlayClearAnim() {
			stages.Clear( currentStage.index );
			popup.SetScroll( currentStage );
			yield return new WaitUntilAnimFinish();
			yield return popup.PlayClearAnim( currentStage );

			PlayRandomGame();
		}

		private class WaitUntilAnimFinish : CustomYieldInstruction {
			public override bool keepWaiting {
				get {
					return isFinished == false;
				}
			}

			private bool isFinished = false;

			public WaitUntilAnimFinish() {
				InGameEvents.OnAnimFinished += OnFinishAnim;
			}

			private void OnFinishAnim() {
				isFinished = true;
				InGameEvents.OnAnimFinished -= OnFinishAnim;
			}
		}
	}
}