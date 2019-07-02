using UnityEngine;
using System.Collections;
using Summoner.UI;

namespace Summoner.FreeCell {
	public class StageSelector : MonoBehaviour {
		[SerializeField] private int testStage = -1;
		[SerializeField] private StagePopup popup = null;
		[UnityEngine.Serialization.FormerlySerializedAs( "onChangeSeed" )]
		[SerializeField] private PresentInt presentStageNumber = null;
		[SerializeField] private PresentToggle presentCleared = null;

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

		public bool OnClear() {
			if ( stages.IsCleared( currentStage ) == true ) {
				return false;
			}

			stages.Clear( currentStage );
			return true;
		}

		private void OnNewGame( StageNumber stageNumber ) {
			currentStage = stageNumber;
			presentCleared.Invoke( stages.IsCleared( stageNumber ) );
			presentStageNumber.Invoke( stageNumber );
			popup.SetScroll( stageNumber );
		}

		public void PlayRandomGame() {
			var randomStage = DrawRandomStage();
			InGameEvents.NewGame( randomStage );
		}

		private StageNumber DrawRandomStage() {
#if UNITY_EDITOR
			if ( testStage > 0 ) {
				return StageNumber.FromStageNumber( testStage );
			}
#endif
			var randomIndex = Random.Range( 0, StageInfo.numStages );
			return StageNumber.FromIndex( randomIndex );
		}
	}
}