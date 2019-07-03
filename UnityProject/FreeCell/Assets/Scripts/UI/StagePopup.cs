using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Summoner.UI;
using Summoner.UI.Popups;

namespace Summoner.FreeCell {
	public class StagePopup : SimplePopup {
		[SerializeField] private DynamicGridLayoutGroup grid = null;
		[SerializeField] private Animator popupAnim = null;
		[SerializeField] private Graphic blocker = null;
		[SerializeField] private PresentRatio presentCleared = null;
		[SerializeField] private UnityEvent onSelectStage = null;

		private static readonly int animParam = Animator.StringToHash( "Current" );
		private static readonly int readyParam = Animator.StringToHash( "Ready" );

		private IStageStatesReader stages;

		void Reset() {
			grid = GetComponentInChildren<DynamicGridLayoutGroup>();
			popupAnim = GetComponentInParent<Animator>();
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
			popupAnim.SetBool( readyParam, true );
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
			popupAnim.SetInteger( animParam, 2 );
			item.ReadyForClearAnim();
			yield return null;
			yield return new WaitWhile( () => ( popupAnim.IsInTransition( 3 ) ) );
			PresentCleared();
			yield return item.PlayClearAnim();
			popupAnim.SetInteger( animParam, -1 );
			blocker.raycastTarget = false;
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