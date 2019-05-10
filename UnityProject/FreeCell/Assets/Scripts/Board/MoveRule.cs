using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Summoner.Util.Extension;
using PileType = Summoner.FreeCell.PileId.Type;

namespace Summoner.FreeCell {
	public class MoveRule {
		private readonly IBoardController board;
		public MoveRule( IBoardController board ) {
			this.board = board;
		}

		private Card lastClicked = Card.Blank;
		private PileTraverser traverser = null;

		public void AutoMove( SelectPosition selected ) {
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

			foreach ( var moveTo in traverser.Traverse( selected.pile ) ) {
				var pile = moveTo.pile;
				var isFreeToFree = selected.type == PileType.Free 
								&& moveTo.id.type == PileType.Free;
				if ( isFreeToFree == true ) {
					continue;
				}

				if ( pile.IsAcceptable( poped ) == false ) {
					continue;
				}

				var adjustment = moveTo.isEmptyTableau ? 2 : 1;
				if ( poped.Length > numMovable / adjustment ) {
					continue;
				}

				pile.Push( poped );
				InGameEvents.MoveCards( poped, moveTo.id );
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

		private class PileTraverser {
			private readonly IList<Pile> homes;
			private readonly IList<Pile> piles;

			public PileTraverser( IBoardController board, PileId selected ) {
				var homes = new List<Pile>( 4 );
				homes.AddRange( Convert( board, PileType.Home ) );
				this.homes = homes.AsReadOnly();

				var piles = new List<Pile>( 12 );
				piles.AddRange( Convert( board, PileType.Table ) );
				piles.AddRange( Convert( board, PileType.Free ) );
				if ( selected.type == PileType.Table ) {
					var selectedPile = piles[selected.index];
					piles.RemoveAt( selected.index );
					piles.Add( selectedPile );
				}
				this.piles = piles.AsReadOnly();
			}

			private static IEnumerable<Pile> Convert( IBoardController board, PileType type ) {
				var piles = board[type];
				for ( int i = 0; i < piles.Count; ++i ) {
					yield return new Pile( piles[i], type, i );
				}
			}

			public IEnumerable<Pile> Traverse( PileId selected ) {
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

		private struct Pile {
			public readonly IPile pile;
			public readonly PileId id;

			public bool isEmptyTableau {
				get {
					return pile is Tableau
						&& pile.Count == 0;
				}
			}

			public Pile( IPile pile, PileId id ) {
				this.pile = pile;
				this.id = id;
			}

			public Pile( IPile pile, PileId.Type type, int index ) 
				: this( pile, new PileId( type, index ) ){ }
		}
	}
}