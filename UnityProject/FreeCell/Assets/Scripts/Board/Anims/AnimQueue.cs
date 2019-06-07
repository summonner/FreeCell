using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Summoner.Util.Coroutine;

namespace Summoner.FreeCell.Anims {
	public class AnimQueue : SubBehaviour {
		public AnimQueue( MonoBehaviour outer )
			: base( outer ) { }

		private Queue<AnimTrigger> anims = new Queue<AnimTrigger>( 52 );
		private CoroutineController scheduler = CoroutineController.Emptied;

		public bool isPlaying {
			get	{
				return scheduler.isRunning;
			}
		}

		public void Enqueue( System.Action trigger, float delay ) {
			var animTrigger = new AnimTrigger( trigger, delay );
			anims.Enqueue( animTrigger );

			Play();
		}

		public void Enqueue( IEnumerable<System.Action> triggers, float delay ) {
			foreach ( var trigger in triggers ) {
				Enqueue( trigger, delay );
			}
		}

		public void Enqueue( float delay ) {
			Enqueue( delegate { }, delay );
		}

		private void Play() {
			if ( isPlaying == true ) {
				return;
			}

			scheduler = new CoroutineController( ScheduleAnim( anims ) );
			StartCoroutine( scheduler );
		}

		private static IEnumerator ScheduleAnim( Queue<AnimTrigger> triggers ) {
			while ( triggers.Count > 0 ) {
				var anim = triggers.Dequeue();
				anim.play();
				if ( anim.delay > 0f ) {
					yield return new WaitForSeconds( anim.delay );
				}
			}
		}

		public void ResetDelays( float interval ) {
			foreach ( var anim in anims ) {
				anim.delay = interval;
			}
		}

		public void Clear() {
			scheduler.Stop();
			anims.Clear();
		}
	}
}