using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public static class InGameEvents {
		public delegate void SetEvent( Card subject, PileId to );
		public static event SetEvent OnSetCard = delegate { };
		public static void SetCard( Card subject, PileId to ) {
			OnSetCard( subject, to );
		}

		public delegate void MoveEvent( IEnumerable<Card> subjects, PileId from, PileId to );
		public static event MoveEvent OnMoveCards = delegate { };
		public static void MoveCards( IEnumerable<Card> subjects, PileId from, PileId to ) {
			OnMoveCards( subjects, from, to );
		}

		public delegate void ClickEvent( SelectPosition selected );
		public static event ClickEvent OnClickCard = delegate { };
		public static void ClickCard( SelectPosition selected ) {
			OnClickCard( selected );
		}

		public delegate void CannotMoveEvent( IEnumerable<Card> subjects );
		public static event CannotMoveEvent OnCannotMove = delegate { };
		public static void CannotMove( IEnumerable<Card> subjects ) {
			OnCannotMove( subjects );
		}

		public static void Flush() {
			OnSetCard = delegate { };
			OnMoveCards = delegate { };
			OnClickCard = delegate { };
			OnCannotMove = delegate { };
		}
	}
}