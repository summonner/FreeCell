using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Summoner.Util.Extension;
using PileType = Summoner.FreeCell.PileId.Type;

namespace Summoner.FreeCell {
	public class SmartMove : IRuleComponent {
		private readonly IBoardController board;
		private readonly CardMover mover;
		public SmartMove( IBoardController board ) {
			this.board = board;
			this.mover = new CardMover( board );

			PlayerInputEvents.OnClick += MoveSmart;
			InGameEvents.OnUndoCards += OnUndo;
		}

		public void Dispose() {
			PlayerInputEvents.OnClick -= MoveSmart;
			InGameEvents.OnUndoCards -= OnUndo;
		}

		public void Reset() {
			lastClicked = Card.Blank;
			traverser = null;
		}

		private Card lastClicked = Card.Blank;
		private PileTraverser traverser = null;

		private void MoveSmart( PositionOnBoard selected ) {
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

		private void OnUndo( ICollection<Card> subjects, PileId from, PileId to ) {
			Reset();
		}

		private class PileTraverser {
			private readonly IList<PileId> homes;
			private readonly IList<PileId> piles;

			public PileTraverser( IBoardController board, PileId selected ) {
				homes = board[PileType.Home].Select( (pile) => ( pile.id ) ).ToList().AsReadOnly();

				piles = (from pile in board[PileType.Table, PileType.Free]
						orderby (selected.type == PileType.Table && pile.id != selected),	// false is first
								pile.id.type,
								pile.Count == 0,
								pile.id.index
						select pile.id)
						.ToList().AsReadOnly();
			}

			public IEnumerable<PileId> Traverse( PileId selected ) {
				foreach ( var home in homes ) {
					yield return home;
				}

				var start = piles.FindIndex( (pile) => ( pile == selected ) );
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
									&& pile.type == PileType.Free;
					if ( isFreeToFree == true ) {
						continue;
					}

					yield return pile;
				}
			}
		}
	}
}