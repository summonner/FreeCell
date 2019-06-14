using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public class BoardHasher {
		private readonly IBoardLookup board;
		public BoardHasher( IBoardLookup board ) {
			this.board = board;
		}

		public string Generate() {
			var tableau = SerializeTableau();
			var freecells = SerializeFreeCells();
			var bytes = tableau.Concat( freecells ).ToArray();
			return System.Convert.ToBase64String( bytes );
		}

		private IEnumerable<byte> SerializeTableau() {
			var tableau = GetPiles( PileId.Type.Table );
			foreach ( var pile in tableau ) {
				yield return (byte)pile.Count;
				foreach ( var card in pile ) {
					yield return (byte)card.GetHashCode();
				}
			}
		}

		private IEnumerable<byte> SerializeFreeCells() {
			var freecells = GetPiles( PileId.Type.Free );
			foreach ( var pile in freecells ) {
				if ( pile.Count <= 0 ) {
					continue;
				}

				yield return (byte)pile[0].GetHashCode();
			}
		}

		private IEnumerable<IPileLookup> GetPiles( PileId.Type type ) {
			return board[type].OrderBy( (pile) => ( pile.FirstOrDefault().GetHashCode() ) );
		}
	}
}