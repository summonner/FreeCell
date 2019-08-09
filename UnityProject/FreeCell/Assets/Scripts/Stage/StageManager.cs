using UnityEngine;
using System.Collections;
using Summoner.UI;
using Summoner.Util;

namespace Summoner.FreeCell {
	public class StageManager : MonoBehaviour {
		[SerializeField] private int testStage = -1;
		[SerializeField] private StagePopup popup = null;
		[SerializeField] private PresentInt presentStageNumber = null;
		[SerializeField] private PresentToggle presentCleared = null;

		private StageStates stages = null;
		private SavedStageNumber currentStage;
		private StageSelector selector;

		public StageNumber current {
			get {
				return currentStage;
			}
		}

		void Awake() {
			stages = new StageStates();
			popup.Init( stages );
			selector = new StageSelector( stages );

			InGameEvents.OnNewGame += PresentStageInfos;
			InGameUIEvents.OnCloseTitle += PlayLastGame;
			InGameUIEvents.OnQuickGame += PlayQuickGame;
		}

		void OnDestroy() {
			stages.Dispose();

			InGameEvents.OnNewGame -= PresentStageInfos;
			InGameUIEvents.OnCloseTitle -= PlayLastGame;
			InGameUIEvents.OnQuickGame -= PlayQuickGame;
		}

		public bool OnClear() {
			if ( stages.IsCleared( currentStage ) == true ) {
				return false;
			}

			stages.Clear( currentStage );
			return true;
		}

		private void PresentStageInfos( StageNumber stageNumber ) {
			if ( currentStage != null ) {
				currentStage.value = stageNumber;
			}

			presentCleared.Invoke( stages.IsCleared( stageNumber ) );
			presentStageNumber.Invoke( stageNumber );
			popup.SetScroll( stageNumber );
		}

		private void PlayLastGame() {
			currentStage = selector.GetLastStage();
			PlayNewGame( currentStage.value );
		}

		public void PlayQuickGame() {
			var newStage = selector.SelectNewStage( currentStage );
			PlayNewGame( newStage );
		}

		private void PlayNewGame( StageNumber newStage ) {
#if UNITY_EDITOR
			if ( testStage > 0 ) {
				newStage = StageNumber.FromStageNumber( testStage );
			}
#endif
			InGameEvents.NewGame( newStage );
		}

		public void OnUseCloud( bool useCloud ) {
			if ( stages == null ) {
				return;
			}

			stages.OnUseCloud( useCloud );
			popup.Refresh();
		}
	}
}