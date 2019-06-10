using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public class AutoPlayToHome : IRuleComponent {
		private readonly IBoardController board;

		public AutoPlayToHome( IBoardController board ) {
			this.board = board;
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

					var poped = board[pile.id].Pop( pile.Count - 1 );
					Debug.Assert( poped.IsNullOrEmpty() == false );
					home.Push( poped );
					InGameEvents.AutoPlay( poped, pile.id, home.id );
					return;
				}
			}
		}

		private static readonly Card.Rank[] specialCases = new [] { Card.Rank.Ace, Card.Rank._2 };
		private bool DoesExistAnyStackable( Card card ) {
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
	}
}