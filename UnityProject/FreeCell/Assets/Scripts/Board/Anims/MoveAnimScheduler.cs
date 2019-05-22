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
			InGameEvents.OnMoveCards += OnMoveCards;
			InGameEvents.OnUndoCards += OnMoveCards;
			InGameEvents.OnAutoPlay += OnAutoPlay;
			InGameEvents.OnClear += OnClear;

			autoPlayQueue = new AnimQueue( this );
		}

		void OnDestroy() {
			InGameEvents.OnInitBoard -= OnInitBoard;
			InGameEvents.OnMoveCards -= OnMoveCards;
			InGameEvents.OnUndoCards -= OnMoveCards;
			InGameEvents.OnAutoPlay -= OnAutoPlay;
			InGameEvents.OnClear -= OnClear;
		}

		public static IEnumerator ScheduleAnim( Queue<AnimTrigger> triggers, System.Action onFinish ) {
			while ( triggers.Count > 0 ) {
				var anim = triggers.Dequeue();
				anim.play();
				yield return new WaitForSeconds( anim.delay );
			}

			onFinish();
		}

		private void OnMoveCards( IEnumerable<Card> targets, PileId from, PileId to ) {
			var userQueue = new Queue<AnimTrigger>();
			foreach ( var target in targets ) {
				var trigger = placer.MoveCard( target, to );
				userQueue.Enqueue( new AnimTrigger( trigger, shortInterval ) );
			}
			
			StartCoroutine( ScheduleAnim( userQueue, delegate { } ) );

			if ( autoPlayQueue.isPlaying == false ) {
				autoPlayQueue.Enqueue( longInterval );
			}
		}

		private void OnInitBoard( Card target, PileId to ) {
			var trigger = placer.MoveCard( target, to );
			autoPlayQueue.Enqueue( trigger, shortInterval );
		}

		private void OnAutoPlay( ICollection<Card> targets, PileId from, PileId to ) {
			foreach ( var target in targets ) {
				var trigger = placer.MoveCard( target, to );
				autoPlayQueue.Enqueue( trigger, shortInterval );
			}
			autoPlayQueue.Enqueue( longInterval - shortInterval );
		}

		private void OnClear() {
			autoPlayQueue.ResetDelays( shortInterval );
		}
	}
}