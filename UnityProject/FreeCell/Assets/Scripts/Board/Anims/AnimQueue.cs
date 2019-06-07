using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell.Anims {
	public class AnimQueue : Util.SubBehaviour {
		public AnimQueue( MonoBehaviour outer )
			: base( outer ) { }

		private Queue<AnimTrigger> anims = new Queue<AnimTrigger>( 52 );
		private Coroutine scheduler;

		public bool isPlaying
		{
			get
			{
				return scheduler != null;
			}
		}

		public void Enqueue( System.Action trigger, float delay ) {
			var animTrigger = new AnimTrigger( trigger, delay );
			anims.Enqueue( animTrigger );

			Play();
		}

		public void Enqueue( float delay ) {
			Enqueue( null, delay );
		}

		private void Play() {
			if ( isPlaying == true ) {
				return;
			}

			scheduler = StartCoroutine( MoveAnimScheduler.ScheduleAnim( anims, () => { scheduler = null; } ) );
		}

		public void ResetDelays( float interval ) {
			foreach ( var anim in anims ) {
				anim.delay = interval;
			}
		}

		public void Clear() {
			StopCoroutine( scheduler );
			scheduler = null;
			anims.Clear();
		}
	}
}