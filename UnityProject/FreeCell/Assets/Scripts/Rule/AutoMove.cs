using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Summoner.Util.Extension;
using PileType = Summoner.FreeCell.PileId.Type;

namespace Summoner.FreeCell {
	public class AutoMove : IRuleComponent {
		private readonly IBoardController board;
		private readonly CardMover mover;
		public AutoMove( IBoardController board ) {
			this.board = board;
			this.mover = new CardMover( board );

			PlayerInputEvents.OnClick += OnAutoMove;
		}

		public void Dispose() {
			PlayerInputEvents.OnClick -= OnAutoMove;
		}

		public void Reset() {
			// do nothing
		}

		private Card lastClicked = Card.Blank;
		private PileTraverser traverser = null;

		private void OnAutoMove( PositionOnBoard selected ) {
			if ( mover.SetSource( selected ) == false ) {
				return;
			}

			var clicked = mover.clicked;
			if ( lastClicked != clicked ) {
				traverser = new PileTraverser( board, selected.pile );
				lastClicked = clicked;
			}

			var destinations = traverser.Traverse( selected.pile );
			mover.Execute( destinations );
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
					var pile = piles[index];
					var isFreeToFree = selected.type == PileType.Free
									&& pile.id.type == PileType.Free;
					if ( isFreeToFree == true ) {
						continue;
					}

					yield return pile;
				}
			}

			private IList<IPile> FindCandidates( PileType type ) {
				if ( type == PileType.Free ) {
					return piles.FindAll( (pile) => ( pile.id.type != PileType.Free ) );
				}
				else {
					return piles;
				}
			}
		}
	}
}