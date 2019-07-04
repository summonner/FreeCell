using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.FreeCell.Anims {
	public class MoveAnimScheduler : MonoBehaviour {
		[SerializeField][Range( 0.01f, 1f )] private float longInterval = 0.1f;
		[SerializeField][Range( 0f, 0.1f )]  private float shortInterval = 0.01f;
		[SerializeField][Range( 0f, 0.1f )] private float groupInterval = 0f;
		[SerializeField][Range( 0f, 1f )] private float waitAfterReset = 0.2f;
		[SerializeField][Range( 0f, 0.1f )] private float clearInterval = 0.03f;
		[SerializeField][Range( 0f, 1f )] private float initVolume = 0.5f;

		[SerializeField] private CardObjectHolder placer;
		private AnimQueue autoPlayQueue;

		void Reset() {
			placer = GetComponent<CardObjectHolder>();
		}

		void Awake() {
			InGameEvents.OnInitBoard += OnInitBoard;
			InGameEvents.OnClearBoard += OnReset;
			InGameEvents.OnPlayerMove += MoveCardAndCheckAutoPlay;
			InGameEvents.OnUndoCards += MoveCards;
			InGameEvents.OnAutoPlay += OnAutoPlay;

			autoPlayQueue = new AnimQueue( this );
		}

		void OnDestroy() {
			InGameEvents.OnInitBoard -= OnInitBoard;
			InGameEvents.OnClearBoard -= OnReset;
			InGameEvents.OnPlayerMove -= MoveCardAndCheckAutoPlay;
			InGameEvents.OnUndoCards -= MoveCards;
			InGameEvents.OnAutoPlay -= OnAutoPlay;
		}

		private void MoveCardAndCheckAutoPlay( IEnumerable<Card> targets, PileId from, PileId to ) {
			MoveCards( targets, from, to );
			if ( autoPlayQueue.isPlaying == false ) {
				autoPlayQueue.Enqueue( longInterval );
				autoPlayQueue.Enqueue( InGameEvents.CheckAutoPlay, 0 );
			}
		}

		private void MoveCards( IEnumerable<Card> targets, PileId from, PileId to ) {
			var userQueue = new AnimQueue( this );
			var trigger = placer.MoveCard( targets, to );
			userQueue.Enqueue( trigger, groupInterval );
		}

		private void OnInitBoard( Card target, PileId to ) {
			var trigger = placer.MoveCard( target, to, initVolume );
			autoPlayQueue.Enqueue( trigger, shortInterval );
		}

		private void OnAutoPlay( ICollection<Card> targets, PileId from, PileId to ) {
			var trigger = placer.MoveCard( targets, to );
			autoPlayQueue.Enqueue( trigger, groupInterval );
			autoPlayQueue.Enqueue( longInterval - groupInterval );
			autoPlayQueue.Enqueue( InGameEvents.CheckAutoPlay, 0 );
		}

		public Coroutine OnClear() {
			return StartCoroutine( OverrideAnimDelay() );
		}

		private IEnumerator OverrideAnimDelay() {
			autoPlayQueue.overrideDelay = clearInterval;
			yield return new WaitWhile( () => ( autoPlayQueue.isPlaying ) );
			autoPlayQueue.overrideDelay = 0f;
		}

		private void OnReset() {
			autoPlayQueue.Clear();
			autoPlayQueue.Enqueue( placer.OnReset(), 0 );
			autoPlayQueue.Enqueue( waitAfterReset );
		}
	}
}