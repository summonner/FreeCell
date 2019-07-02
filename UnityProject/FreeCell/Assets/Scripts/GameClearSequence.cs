using UnityEngine;
using System.Collections;
using Summoner.UI.Tween;
using Summoner.FreeCell.Anims;

namespace Summoner.FreeCell {
	public class GameClearSequence : MonoBehaviour {
		[SerializeField] private MoveAnimScheduler scheduler = null;
		[SerializeField] private TweenBase congraturation = null;
		[SerializeField] private StagePopup stagePopup = null;
		[SerializeField] private StageSelector stageSelector = null;

		void Reset() {
			scheduler = FindObjectOfType<MoveAnimScheduler>();
			stagePopup = FindObjectOfType<StagePopup>();
			congraturation = GameObject.Find( "Canvas/GameUI/Messages/Congraturation" ).GetComponent<TweenBase>();
			stageSelector = FindObjectOfType<StageSelector>();
			
		}

		void Awake() {
			InGameEvents.OnGameClear += OnClear;
		}

		void OnDestroy() {
			InGameEvents.OnGameClear -= OnClear;
		}

		private void OnClear() {
			StartCoroutine( PlaySequence() );
		}

		private IEnumerator PlaySequence() {
			var doesShowPopup = stageSelector.OnClear();
			yield return scheduler.OnClear();
			yield return congraturation.Play();
			yield return new WaitForSeconds( 1f );

			if ( doesShowPopup == true ) {
				yield return StartCoroutine( stagePopup.PlayClearAnim( stageSelector.currentStage ) );
				congraturation.value = 0;
			}
			else {
				congraturation.PlayReverse();
			}

			stageSelector.PlayRandomGame();
		}
	}
}