using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public static class InGameEvents {
		public delegate void MoveEvent( IEnumerable<Card> targets, PileId destination );
		public static MoveEvent OnMoveCards = delegate { };
		public static void MoveACard( Card target, PileId destination ) {
			OnMoveCards( new [] { target }, destination );
		}
	}
}