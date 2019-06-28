using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Summoner.FreeCell.Anims {
	public class MoveAnimScheduler : MonoBehaviour {
		[SerializeField][Range( 0.01f, 1f )] private float longInterval = 0.1f;
		[SerializeField][Range( 0f, 0.1f )]  private float shortInterval = 0.01f;

		[SerializeField] private CardObjectHolder placer;
		private AnimQueue autoPlayQueue;

		void Reset() {
			placer = GetComponent<CardObjectHolder>();
		}

		void Awake() {
			InGameEvents.OnInitBoard += OnInitBoard;
			InGameEvents.OnClearBoard += OnReset;
			InGameEvents.OnMoveCards += OnMoveCards;
			InGameEvents.OnUndoCards += OnMoveCards;
			InGameEvents.OnAutoPlay += OnAutoPlay;
			InGameEvents.OnGameClear += OnClear;

			autoPlayQueue = new AnimQueue( this );
		}

		void OnDestroy() {
			InGameEvents.OnInitBoard -= OnInitBoard;
			InGameEvents.OnClearBoard -= OnReset;
			InGameEvents.OnMoveCards -= OnMoveCards;
			InGameEvents.OnUndoCards -= OnMoveCards;
			InGameEvents.OnAutoPlay -= OnAutoPlay;
			InGameEvents.OnGameClear -= OnClear;
		}

		private void OnMoveCards( IEnumerable<Card> targets, PileId from, PileId to ) {
			var userQueue = new AnimQueue( this );
			var trigger = placer.MoveCard( targets, to );
			userQueue.Enqueue( trigger, shortInterval );
			
			if ( autoPlayQueue.isPlaying == false ) {
				autoPlayQueue.Enqueue( longInterval );
			}
		}

		private void OnInitBoard( Card target, PileId to ) {
			var trigger = placer.MoveCard( target, to );
			autoPlayQueue.Enqueue( trigger, shortInterval );
		}

		private void OnAutoPlay( ICollection<Card> targets, PileId from, PileId to ) {
			var trigger = placer.MoveCard( targets, to );
			autoPlayQueue.Enqueue( trigger, shortInterval );
			autoPlayQueue.Enqueue( longInterval - shortInterval );
		}

		private void OnClear() {
			autoPlayQueue.ResetDelays( shortInterval );
			autoPlayQueue.Enqueue( 1f );
			autoPlayQueue.Enqueue( placer.OnReset(), 0 );
			autoPlayQueue.Enqueue( InGameEvents.AnimFinished, 0 );
		}

		private void OnReset() {
			autoPlayQueue.Clear();
			autoPlayQueue.Enqueue( placer.OnReset(), 0 );
			autoPlayQueue.Enqueue( longInterval );
		}
	}
}