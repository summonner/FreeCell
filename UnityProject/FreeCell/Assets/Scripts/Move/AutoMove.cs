using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Summoner.Util.Extension;
using PileType = Summoner.FreeCell.PileId.Type;

namespace Summoner.FreeCell {
	public class AutoMove : System.IDisposable {
		private readonly IBoardController board;
		public AutoMove( IBoardController board ) {
			this.board = board;
			InGameEvents.OnClickCard += OnAutoMove;
		}

		public void Dispose() {
			InGameEvents.OnClickCard -= OnAutoMove;
		}

		private Card lastClicked = Card.Blank;
		private PileTraverser traverser = null;

		private void OnAutoMove( SelectPosition selected ) {
			if ( selected.type == PileType.Home ) {
				return;
			}

			var numMovable = CountMaxMovableCards();
			var selectedPile = board[selected.pile];
			var poped = selectedPile.Pop( selected.row );
			if ( poped.IsNullOrEmpty() == true ) {
				return;
			}

			var clicked = poped[0];
			if ( lastClicked != clicked ) {
				traverser = new PileTraverser( board, selected.pile );
				lastClicked = clicked;
			}

			foreach ( var pile in traverser.Traverse( selected.pile ) ) {
				var isFreeToFree = selected.type == PileType.Free 
								&& pile.id.type == PileType.Free;
				if ( isFreeToFree == true ) {
					continue;
				}

				if ( pile.IsAcceptable( poped ) == false ) {
					continue;
				}

				var adjustment = IsEmptyTableau( pile ) ? 2 : 1;
				if ( poped.Length > numMovable / adjustment ) {
					continue;
				}

				pile.Push( poped );
				InGameEvents.MoveCards( poped, selected.pile, pile.id );
				return;
			}

			selectedPile.Push( poped );
			InGameEvents.CannotMove( poped );
		}

		private int CountMaxMovableCards() {
			var numEmptyFrees = CountEmpties( PileType.Free );
			var numEmptyTableau = CountEmpties( PileType.Table );
			return (1 + numEmptyFrees) * (int)Mathf.Pow( 2f, numEmptyTableau );
		}

		private int CountEmpties( PileType type ) {
			var piles = board[type];
			return piles.Count( ( pile ) => (pile.Count == 0) );
		}

		public bool IsEmptyTableau( IPile pile ) {
			return pile is Tableau
				&& pile.Count == 0;
		}

		private class PileTraverser {
			private readonly IList<IPile> homes;
			private readonly IList<IPile> piles;

			public PileTraverser( IBoardController board, PileId selected ) {
				var homes = new List<IPile>( 4 );
				homes.AddRange( board[PileType.Home] );
				this.homes = homes.AsReadOnly();

				var piles = new List<IPile>( 12 );
				piles.AddRange( board[PileType.Table, PileType.Free] );
				piles.Sort( Sort );
				if ( selected.type == PileType.Table ) {
					MoveSelectedToLast( piles, selected );
				}
				this.piles = piles.AsReadOnly();
			}

			private int Sort( IPile left, IPile right ) {
				if ( left.id.type != right.id.type ) {
					return left.id.type - right.id.type;
				}

				if ( left.Count == 0 && right.Count != 0 ) {
					return 1;
				}
				else if ( left.Count != 0 && right.Count == 0 ) {
					return -1;
				}

				return left.id.index - right.id.index;
			}

			private static void MoveSelectedToLast( List<IPile> piles, PileId selected ) {
				var selectedPile = piles.First( ( pile ) => (pile.id == selected) );
				piles.Remove( selectedPile );
				piles.Add( selectedPile );
			}

			public IEnumerable<IPile> Traverse( PileId selected ) {
				foreach ( var home in homes ) {
					yield return home;
				}

				var start = piles.FindIndex( (pile) => ( pile.id == selected ) );
				var doesnotFound = start < 0
								&& piles.Count > 0;
				if ( doesnotFound == true ) {
					yield return piles.First();
					start = 0;
				}

				for ( int i=1; i < piles.Count; ++i ) {
					var index = (start + i) % piles.Count;
					yield return piles[index];
				}
			}
		}
	}
}