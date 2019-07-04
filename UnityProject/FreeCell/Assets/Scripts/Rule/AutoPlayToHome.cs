using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public class AutoPlayToHome : IRuleComponent {
		private readonly IBoardController board;
		private readonly PossibleMoveFinder moveFinder;

		public AutoPlayToHome( IBoardController board ) {
			this.board = board;
			this.moveFinder = new PossibleMoveFinder( board.AsReadOnly() );

			InGameEvents.OnCheckAutoPlay += CheckAutoPlay;
		}

		public void Dispose() {
			InGameEvents.OnCheckAutoPlay -= CheckAutoPlay;
		}

		public void Reset() {
			// do nothing
		}

		private void CheckAutoPlay() {
			if ( FindMoveToHome() == true ) {
				return;
			}

			if ( moveFinder.HasAnyMove() == false ) {
				InGameEvents.NoMoreMoves();
			}
		}

		private bool FindMoveToHome() {
			foreach ( var move in moveFinder.FindMoves( PileId.Type.Home ) ) {
				if ( DoesExistAnyStackable( move.cards.FirstOrDefault() ) == true ) {
					continue;
				}

				ApplyMove( move );
				return true;
			}

			return false;
		}

		private static readonly Card.Rank[] specialCases = new [] { Card.Rank.Ace, Card.Rank._2 };
		private bool DoesExistAnyStackable( Card card ) {
			if ( card == Card.Blank ) {
				return true;
			}

			if ( specialCases.Contains( card.rank ) == true ) {
				return false;
			}

			return TraverseCards().Any( (tableau) => ( Tableau.IsStackable( tableau, card ) ) );
		}

		private IEnumerable<Card> TraverseCards() {
			foreach ( var pile in board[PileId.Type.Table, PileId.Type.Free] ) {
				foreach ( var card in pile ) {
					yield return card;
				}
			}
		}

		private void ApplyMove( Move move ) {
			var from = board[move.from];
			var poped = from.Pop( from.Count - 1 );
			Debug.Assert( poped.SequenceEqual( move.cards ) );

			board[move.to].Push( poped );

			InGameEvents.AutoPlay( poped, move.from, move.to );
		}
	}
}