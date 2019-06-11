using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public class AutoPlayToHome : IRuleComponent {
		private readonly IBoardController board;
		private readonly MoveTester tester;
		private readonly PossibleMoveFinder moveFinder;

		public AutoPlayToHome( IBoardController board ) {
			this.board = board;
			this.tester = new MoveTester( board.AsReadOnly() );
			this.moveFinder = new PossibleMoveFinder( board.AsReadOnly() );

			InGameEvents.OnMoveCards += OnMoveCards;
			InGameEvents.OnAutoPlay += OnMoveCards;
		}

		public void Dispose() {
			InGameEvents.OnMoveCards -= OnMoveCards;
			InGameEvents.OnAutoPlay -= OnMoveCards;
		}

		public void Reset() {
			// do nothing
		}

		private void OnMoveCards( IEnumerable<Card> _does, PileId _not, PileId _use ) {
			var homes = board[PileId.Type.Home];
			foreach ( var pile in board[PileId.Type.Free, PileId.Type.Table] ) {
				var position = GetLast( pile );
				if ( position.row < 0 ) {
					continue;
				}

				if ( tester.SetSource( position ) != MoveTester.Result.Success ) {
					continue;
				}

				foreach ( var home in homes ) {
					if ( tester.SetDestination( home.id ) != MoveTester.Result.Success ) {
						continue;
					}

					if ( DoesExistAnyStackable( tester.subjects.FirstOrDefault() ) == true ) {
						continue;
					}

					var poped = board[pile.id].Pop( pile.Count - 1 );
					Debug.Assert( poped.IsNullOrEmpty() == false );
					home.Push( poped );
					InGameEvents.AutoPlay( poped, pile.id, home.id );
					return;
				}
			}

			FindAnyPossibleMove( _does, _not, _use );
		}

		private PositionOnBoard GetLast( IPile pile ) {
			return new PositionOnBoard( pile.id, pile.Count - 1 );
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

		private void FindAnyPossibleMove( IEnumerable<Card> cards, PileId from, PileId to ) {
			moveFinder.FindMove( cards, from, to );
		}
	}
}