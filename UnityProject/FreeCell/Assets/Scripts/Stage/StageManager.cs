using UnityEngine;
using System.Threading.Tasks;
using Summoner.UI;

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

			InGameEvents.OnNewGame += OnNewGame;
			InGameUIEvents.OnCloseTitle += PlayLastGame;
			InGameUIEvents.OnQuickGame += PlayQuickGame;
			RefreshStages().WrapError();
		}

		void OnDestroy() {
			stages.Dispose();

			InGameEvents.OnNewGame -= OnNewGame;
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

		private void OnNewGame( StageNumber stageNumber ) {
			if ( currentStage != null ) {
				currentStage.value = stageNumber;
			}

			PresentStageInfos( stageNumber );
		}

		private void PresentStageInfos( StageNumber stageNumber ) {
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

		public async Task RefreshStages() {
			if ( stages == null ) {
				return;
			}

			await stages.Refresh();
			popup.Refresh();

			if ( currentStage != null ) {
				PresentStageInfos( currentStage );
			}
		}
	}
}