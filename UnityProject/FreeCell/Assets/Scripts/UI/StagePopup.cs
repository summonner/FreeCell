using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Summoner.UI;
using Summoner.UI.Popups;

namespace Summoner.FreeCell {
	public class StagePopup : SlidePopup {
		[SerializeField] private DynamicGridLayoutGroup grid = null;
		[SerializeField] private Graphic blocker = null;
		[SerializeField] private PresentRatio presentCleared = null;
		[SerializeField] private UnityEvent onSelectStage = null;
		[SerializeField] private BondUI follower = null;

		private IStageStatesReader stages;

		void Reset() {
			grid = GetComponentInChildren<DynamicGridLayoutGroup>();
			follower = GetComponent<BondUI>();
		}

		public void Init( IStageStatesReader stages ) {
			this.stages = stages;
			PresentCleared();

			grid.onInitGridItem += OnInitButton;
			grid.numItems = stages.Count;
			StartCoroutine( Ready() );
		}

		private IEnumerator Ready() {
			yield return new WaitUntil( () => ( grid.isReady ) );
			animController.Ready();
			follower.Init();
		}

		private void PresentCleared() {
			presentCleared.Invoke( stages.numCleared, StageInfo.numStages );
		}

		public void OnInitButton( int index, GameObject target ) {
			var button = target.GetComponent<StageButton>();
			button.Set( new Stage( this, index ) );
		}

		public void SetScroll( StageNumber stageNumber ) {
			grid.Show( stageNumber.index );
		}

		public IEnumerator PlayClearAnim( StageNumber stageNumber ) {
			grid.Show( stageNumber.index );
			var item = grid.GetItem<StageButton>( stageNumber.index );
			if ( item == null ) {
				Debug.LogError( stageNumber + " does not shown" );
				return null;
			}

			return PlayClearAnim( item );
		}

		private IEnumerator PlayClearAnim( StageButton item ) {
			blocker.raycastTarget = true;
			animController.Show( key );
			item.ReadyForClearAnim();
			yield return null;
			yield return new WaitWhile( () => ( animController.isPlaying ) );
			PresentCleared();
			yield return item.PlayClearAnim();
			animController.Show( -1 );
			blocker.raycastTarget = false;
		}

		public void Refresh() {
			grid.SetLayoutVertical();
			PresentCleared();
		}

		private class Stage : StageButton.IContents {
			private readonly StagePopup outer;
			public StageNumber stageNumber { get; private set; }
			public bool isCleared {
				get {
					return outer.stages.IsCleared( stageNumber );
				}
			}

			public Stage( StagePopup outer, int stageIndex ) {
				this.outer = outer;
				this.stageNumber = StageNumber.FromIndex( stageIndex );
			}

			public void OnClick() {
				InGameEvents.NewGame( stageNumber );
				outer.Close();
				outer.onSelectStage.Invoke();
			}
		}
	}
}