using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public class DragAnim {
		private readonly FloatEffect floater;
		private readonly MoveAnim moveAnim;
		public Vector3 startPosition { private get; set; }

		public DragAnim( MoveAnim moveAnim, FloatEffect floater ) {
			this.moveAnim = moveAnim;
			this.floater = floater;
		}

		public void Begin() {
			floater.Begin();
			moveAnim.displacement = Vector3.zero;

			if ( moveAnim.isPlaying == true ) {
				return;
			}

			startPosition = floater.position;
		}

		public void Move( Vector3 displacement ) {
			if ( moveAnim.isPlaying == true ) {
				moveAnim.displacement = displacement;
			}
			else {
				floater.position = startPosition + displacement;
			}
		}

		public void End( float effectVolume ) {
			floater.End();
			moveAnim.displacement = Vector3.zero;
			moveAnim.SetDestinationImmediate( startPosition, effectVolume );
		}
	}
}