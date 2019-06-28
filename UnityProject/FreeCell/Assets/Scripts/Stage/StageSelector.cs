using UnityEngine;
using System.Collections;
using Summoner.UI;

namespace Summoner.FreeCell {
	public class StageSelector : MonoBehaviour {
		[SerializeField] private int testSeed = -1;
		[SerializeField] private StagePopup popup = null;
		[SerializeField] private PresentInt onChangeSeed = null;

		private StageStates stages = null;
		public StageNumber currentStage { get; private set; }

		void Awake() {
			stages = new StageStates();
			popup.Init( stages );

			InGameEvents.OnNewGame += OnNewGame;
			InGameUIEvents.OnCloseTitle += PlayRandomGame;
			InGameUIEvents.OnQuickGame += PlayRandomGame;
		}

		void OnDestroy() {
			stages.Dispose();

			InGameEvents.OnNewGame -= OnNewGame;
			InGameUIEvents.OnCloseTitle -= PlayRandomGame;
			InGameUIEvents.OnQuickGame -= PlayRandomGame;
		}

		public void OnClear() {
			stages.Clear( currentStage );
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
	}
}