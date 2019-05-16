using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public class AutoPlayToHome : IRuleComponent {
		private readonly IBoardController board;

		public AutoPlayToHome( IBoardController board ) {
			this.board = board;
			InGameEvents.OnMoveCards += OnMoveCards;
		}

		public void Dispose() {
			InGameEvents.OnMoveCards -= OnMoveCards;
		}

		public void Reset() {
			// do nothing
		}

		private void OnMoveCards( IEnumerable<Card> cards, PileId from, PileId to ) {
			var homes = board[PileId.Type.Home];
			foreach ( var pile in board[PileId.Type.Free, PileId.Type.Table] ) {
				var top = pile.LastOrDefault();
				foreach ( var home in homes ) {
					if ( home.IsAcceptable( top ) == false ) {
						continue;
					}

					if ( DoesExistAnyStackable( top ) == true ) {
						continue;
					}

					InGameEvents.ClickCard( new SelectPosition( pile.id, pile.Count - 1 ) );
					return;
				}
			}
		}

		private static readonly Card.Rank[] specialCases = new [] { Card.Rank.Ace, Card.Rank._2 };
		private bool DoesExistAnyStackable( Card card ) {
			if ( specialCases.Contains( card.rank ) == true ) {
				return false;
			}

			return TraverseTableau().Any( (tableau) => ( Tableau.IsStackable( tableau, card ) ) );
		}

		private IEnumerable<Card> TraverseTableau() {
			foreach ( var pile in board[PileId.Type.Table, PileId.Type.Free] ) {
				foreach ( var card in pile ) {
					yield return card;
				}
			}
		}
	}
}