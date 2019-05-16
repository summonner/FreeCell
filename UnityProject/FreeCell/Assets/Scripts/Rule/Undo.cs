using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public class Undo : IRuleComponent {
		private struct Command {
			public readonly IEnumerable<Card> cards;
			public readonly PileId from;
			public readonly PileId to;

			public Command( IEnumerable<Card> cards, PileId from, PileId to ) {
				this.cards = cards;
				this.from = from;
				this.to = to;
			}
		}

		private readonly Stack<Command> commands = new Stack<Command>( 100 );
		private readonly IBoardController board;

		public Undo( IBoardController board ) {
			this.board = board;
			InGameEvents.OnMoveCards += OnMoveCards;
			InGameUIEvents.OnUndo += OnUndo;
		}

		public void Dispose() {
			InGameEvents.OnMoveCards -= OnMoveCards;
			InGameUIEvents.OnUndo -= OnUndo;
		}

		public void Reset() {
			Clear();
		}

		public void Clear() {
			commands.Clear();
		}

		private void OnMoveCards( IEnumerable<Card> cards, PileId from, PileId to ) {
			commands.Push( new Command( cards, from, to ) );
		}

		public void OnUndo() {
			if ( commands.Count <= 0 ) {
				return;
			}

			var command = commands.Pop();
			Revert( command.cards, command.from, command.to );
		}

		public void Revert( IEnumerable<Card> targets, PileId to, PileId from ) {
			var cards = new List<Card>( targets );
			var source = board[from];

			var index = source.IndexOf( cards[0] );
			Debug.Assert( index >= 0, "cannot find cards to undo" );
			var poped = source.Pop( index );
			Debug.Assert( cards.Count == poped.Length, "board state mismatch" );
			var destination = board[to];
			destination.Push( poped );

			InGameEvents.UndoCards( poped, from, to );
		}
	}
}