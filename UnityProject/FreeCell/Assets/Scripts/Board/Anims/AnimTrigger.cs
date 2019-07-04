using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell.Anims {
	public class AnimTrigger {
		public readonly float delay;
		public readonly System.Action play;

		public AnimTrigger( System.Action trigger, float delay ) {
			this.play = trigger ?? delegate { };
			this.delay = delay;
		}
	}
}