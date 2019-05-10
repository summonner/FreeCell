using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public class CommandStack : System.IDisposable {
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

		public CommandStack( IBoardController board ) {
			this.board = board;
			RegisterEvent( true );
			InGameUIEvents.OnUndo += OnUndo;
		}

		public void Dispose() {
			RegisterEvent( false );
			InGameUIEvents.OnUndo -= OnUndo;
		}
		
		private void RegisterEvent( bool enable ) {
			if ( enable ) {
				InGameEvents.OnMoveCards += OnMoveCards;
			}
			else {
				InGameEvents.OnMoveCards -= OnMoveCards;
			}
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
			RegisterEvent( false );
			Revert( command.cards, command.from, command.to );
			RegisterEvent( true );
		}

		public void Revert( IEnumerable<Card> targets, PileId to, PileId from ) {
			var cards = new List<Card>( targets );
			var source = board[from];

			var index = source.GetReadOnly().IndexOf( cards[0] );
			Debug.Assert( index >= 0, "cannot find cards to undo" );
			var poped = source.Pop( index );
			Debug.Assert( cards.Count == poped.Length, "board state mismatch" );
			var destination = board[to];
			destination.Push( poped );

			InGameEvents.MoveCards( poped, from, to );
		}
	}
}