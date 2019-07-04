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
		public float overrideDelay = 0f;

		public bool isPlaying {
			get	{
				return scheduler.isRunning;
			}
		}

		public void Enqueue( System.Action trigger, float delay ) {
			var animTrigger = new AnimTrigger( trigger, delay );
			anims.Enqueue( animTrigger );

			if ( isPlaying == false ) {
				Play();
			}
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
			scheduler = new CoroutineController( ScheduleAnim( anims ) );
			StartCoroutine( scheduler );
		}

		private IEnumerator ScheduleAnim( Queue<AnimTrigger> triggers ) {
			while ( triggers.Count > 0 ) {
				var anim = triggers.Dequeue();
				anim.play();
				if ( anim.delay <= 0f ) {
					continue;
				}

				float delay = overrideDelay > 0f ? overrideDelay : anim.delay;
				yield return new WaitForSeconds( delay );
			}
		}

		public void Clear() {
			scheduler.Stop();
			anims.Clear();
		}
	}
}