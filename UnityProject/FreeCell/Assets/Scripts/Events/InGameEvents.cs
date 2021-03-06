using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public static class InGameEvents {
		private static IList<System.Type> requireReady = new List<System.Type>() {
			typeof( StagePopup ),
		};

		public static void Ready( object instigator ) {
			var instigatorType = instigator.GetType();
			Debug.Assert( requireReady.Contains( instigatorType ) );
			requireReady.Remove( instigatorType );
		}

		public static bool IsReadyToStart() {
			return requireReady.Count == 0;
		}

		public delegate void SetEvent( Card subject, PileId to );
		public static event SetEvent OnInitBoard = delegate { };
		public static void InitBoard( Card subject, PileId to ) {
			OnInitBoard( subject, to );
		}

		public delegate void ClearBoardEvent();
		public static event ClearBoardEvent OnClearBoard = delegate { };
		public static void ClearBoard() {
			OnClearBoard();
		}

		public delegate void MoveEvent( ICollection<Card> subjects, PileId from, PileId to );
		public static event MoveEvent OnPlayerMove = delegate { };
		public static event MoveEvent OnMoveCards = delegate { };
		public static void MoveCards( ICollection<Card> subjects, PileId from, PileId to ) {
			OnMoveCards( subjects, from, to );
			OnPlayerMove( subjects, from, to );
		}

		public static event MoveEvent OnUndoCards = delegate { };
		public static void UndoCards( ICollection<Card> subjects, PileId from, PileId to ) {
			OnUndoCards( subjects, from, to );
		}

		public static event MoveEvent OnAutoPlay = delegate { };
		public static void AutoPlay( ICollection<Card> subjects, PileId from, PileId to ) {
			OnMoveCards( subjects, from, to );
			OnAutoPlay( subjects, from, to );
		}

		public static event System.Action OnCheckAutoPlay = delegate { };
		public static void CheckAutoPlay() {
			OnCheckAutoPlay();
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

		public static event System.Action OnGameClear = delegate { };
		public static void GameClear() {
			OnGameClear();
		}

		public static event System.Action OnNoMoreMoves = delegate { };
		public static void NoMoreMoves() {
			OnNoMoreMoves();
		}

		public static event System.Action OnNotEnoughFreeCells = delegate { };
		public static void NotEnoughFreeCells() {
			OnNotEnoughFreeCells();
		}


		public delegate void NewGameEvent( StageNumber stageNumber );
		public static event NewGameEvent OnNewGame = delegate { };
		public static void NewGame( StageNumber stageNumber ) {
			OnNewGame( stageNumber );
		}

		public static void Flush() {
			OnInitBoard = delegate { };
			OnClearBoard = delegate { };
			OnPlayerMove = delegate { };
			OnMoveCards = delegate { };
			OnUndoCards = delegate { };
			OnAutoPlay = delegate { };
			OnCheckAutoPlay = delegate { };
			OnCannotMove = delegate { };
			OnBeginFloatCards = delegate { };
			OnFloatCards = delegate { };
			OnEndFloatCards = delegate { };
			OnGameClear = delegate { };
			OnNoMoreMoves = delegate { };
			OnNotEnoughFreeCells = delegate { };
			OnNewGame = delegate { };
		}
	}
}