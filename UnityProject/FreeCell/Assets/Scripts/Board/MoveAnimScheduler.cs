using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public class MoveAnimScheduler : MonoBehaviour {
		[SerializeField][Range( 0.01f, 1f )] private float longInterval = 0.1f;
		[SerializeField][Range( 0f, 0.1f )]  private float shortInterval = 0.01f;

		[SerializeField] private CardPlacer placer;
		private Queue<AnimTrigger> anims = new Queue<AnimTrigger>( 52 );

		IEnumerator Start() {
			while ( true ) {
				if ( anims.Count <= 0 ) {
					yield return null;
					continue;
				}

				var anim = anims.Dequeue();
				anim.play();
				yield return new WaitForSeconds( anim.delay );
			}
		}

		void Reset() {
			placer = GetComponent<CardPlacer>();
		}

		void Awake() {
			InGameEvents.OnSetCard += OnSetCard;
			InGameEvents.OnMoveCards += OnMoveCards;
			InGameEvents.OnUndoCards += OnMoveCards;
			InGameEvents.OnClear += OnClear;
		}

		void OnDestroy() {
			InGameEvents.OnSetCard -= OnSetCard;
			InGameEvents.OnMoveCards -= OnMoveCards;
			InGameEvents.OnUndoCards -= OnMoveCards;
			InGameEvents.OnClear -= OnClear;
		}

		private void OnMoveCards( IEnumerable<Card> targets, PileId from, PileId to ) {
			foreach ( var target in targets ) {
				OnSetCard( target, to );
			}
			anims.Enqueue( new AnimTrigger( null, longInterval - shortInterval ) );
		}

		private void OnSetCard( Card target, PileId to ) {
			var trigger = placer.MoveCard( target, to );
			anims.Enqueue( new AnimTrigger( trigger, shortInterval ) );
		}

		private void OnClear() {
			foreach ( var anim in anims ) {
				anim.delay = shortInterval;
			}
		}

		private class AnimTrigger {
			public float delay;
			public readonly System.Action play;

			public AnimTrigger( System.Action trigger, float delay ) {
				this.play = trigger ?? delegate { };
				this.delay = delay;
			}
		}
	}
}