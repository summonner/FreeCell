using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public static class InGameEvents {
		public delegate void SetEvent( Card subject, PileId to );
		public static event SetEvent OnInitBoard = delegate { };
		public static void InitBoard( Card subject, PileId to ) {
			OnInitBoard( subject, to );
		}

		public delegate void MoveEvent( ICollection<Card> subjects, PileId from, PileId to );
		public static event MoveEvent OnMoveCards = delegate { };
		public static void MoveCards( ICollection<Card> subjects, PileId from, PileId to ) {
			OnMoveCards( subjects, from, to );
		}

		public static event MoveEvent OnUndoCards = delegate { };
		public static void UndoCards( ICollection<Card> subjects, PileId from, PileId to ) {
			OnUndoCards( subjects, from, to );
		}

		public static event MoveEvent OnAutoPlay = delegate { };
		public static void AutoPlay( ICollection<Card> subjects, PileId from, PileId to ) {
			OnAutoPlay( subjects, from, to );
		}

		public delegate void CannotMoveEvent( ICollection<Card> subjects );
		public static event CannotMoveEvent OnCannotMove = delegate { };
		public static void CannotMove( ICollection<Card> subjects ) {
			OnCannotMove( subjects );
		}

		public delegate void BeginFloatEvent( IEnumerable<Card> subjects );
		public static event BeginFloatEvent OnBeginFloatCards = delegate { };
		public static void BeginFloatCards( IEnumerable<Card> subjects ) {
			OnBeginFloatCards( subjects );
		}

		public delegate void FloatEvent( IEnumerable<Card> subjects, Vector3 displacement );
		public static event FloatEvent OnFloatCards = delegate { };
		public static void FloatCards( IEnumerable<Card> subjects, Vector3 displacement ) {
			OnFloatCards( subjects, displacement );
		}

		public static event BeginFloatEvent OnEndFloatCards = delegate { };
		public static void EndFloatCards( IEnumerable<Card> subjects ) {
			OnEndFloatCards( subjects );
		}

		public static event System.Action OnClear = delegate { };
		public static void Clear() {
			OnClear();
		}

		private static readonly Util.Event.Backup initialValues = null;
		static InGameEvents() {
			initialValues = new Util.Event.Backup( typeof( InGameEvents ) );
		}

		public static void Flush() {
			initialValues.Apply();
		}
	}
}