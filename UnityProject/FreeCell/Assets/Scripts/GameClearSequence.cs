using UnityEngine;
using System.Collections;
using Summoner.UI.Tween;
using Summoner.FreeCell.Anims;
using Summoner.Util.Coroutine;

namespace Summoner.FreeCell {
	public class GameClearSequence : MonoBehaviour {
		[SerializeField] private MoveAnimScheduler scheduler = null;
		[SerializeField] private TweenBase congraturation = null;
		[SerializeField] private StagePopup stagePopup = null;
		[SerializeField] private StageManager stageSelector = null;
		private CoroutineController sequence = CoroutineController.Emptied;

		void Reset() {
			scheduler = FindObjectOfType<MoveAnimScheduler>();
			stagePopup = FindObjectOfType<StagePopup>();
			congraturation = GameObject.Find( "Canvas/GameUI/Messages/Congraturation" ).GetComponent<TweenBase>();
			stageSelector = FindObjectOfType<StageManager>();
		}

		void Awake() {
			InGameEvents.OnGameClear += OnClear;
		}

		void OnDestroy() {
			InGameEvents.OnGameClear -= OnClear;
		}

		private void OnClear() {
			if ( sequence.isRunning == true ) {
				return;
			}

			sequence = new CoroutineController( PlaySequence() );
			StartCoroutine( sequence );
		}

		private IEnumerator PlaySequence() {
			var doesShowPopup = stageSelector.OnClear();
			yield return null;
			yield return scheduler.OnClear();
			yield return congraturation.Play();
			SoundPlayer.Instance.Play( SoundType.Congraturation );
			yield return new WaitForSeconds( 1f );

			if ( doesShowPopup == true ) {
				yield return StartCoroutine( stagePopup.PlayClearAnim( stageSelector.current ) );
				congraturation.value = 0;
			}
			else {
				congraturation.PlayReverse();
			}

			stageSelector.PlayQuickGame();
		}
	}
}