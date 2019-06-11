using UnityEngine;
using System.Linq;

namespace Summoner.FreeCell {
	public struct NumberOfMovables {
		public readonly int value;

		public NumberOfMovables( IBoardLookup board ) {
			var numEmptyFrees = CountEmpties( board, PileId.Type.Free );
			var numEmptyTableau = CountEmpties( board, PileId.Type.Table );
			value = (1 + numEmptyFrees) * (int)Mathf.Pow( 2f, numEmptyTableau );
		}

		private static int CountEmpties( IBoardLookup board, PileId.Type type ) {
			var piles = board[type];
			return piles.Count( ( pile ) => (pile.Count == 0) );
		}

		public int MoveTo( IPileLookup pile ) {
			if ( IsEmptyTableau( pile ) == true ) {
				return value / 2;
			}
			else {
				return value;
			}
		}

		private bool IsEmptyTableau( IPileLookup pile ) {
			return pile.id.type == PileId.Type.Table
				&& pile.Count <= 0;
		}
	}
}