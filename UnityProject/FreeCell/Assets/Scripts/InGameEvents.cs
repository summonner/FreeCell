using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public static class InGameEvents {
		public delegate void MoveEvent( IEnumerable<Card> targets, PileId destination );
		public static event MoveEvent OnMoveCards = delegate { };
		public static void MoveCards( IEnumerable<Card> targets, PileId destination ) {
			OnMoveCards( targets, destination );
		}

		public delegate void ClickEvent( PileId pile, int row );
		public static event ClickEvent OnClickCard = delegate { };
		public static void ClickCard( PileId pile, int row ) {
			OnClickCard( pile, row );
		}

		public static void Flush() {
			OnMoveCards = delegate { };
			OnClickCard = delegate { };
		}
	}
}