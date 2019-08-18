using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public class BoardLayout : MonoBehaviour, IBoardLayout {
		public BoardComponent[] homeCells;
		public BoardComponent[] freeCells;
		public BoardComponent[] tablePiles;

#if UNITY_EDITOR
		private void OnDrawGizmos() {
			DrawGizmos( tablePiles, Color.red );
			DrawGizmos( freeCells, Color.cyan );
			DrawGizmos( homeCells, Color.green );
		}

		private static readonly Vector3 size = new Vector3( 1f, 1.357f, 1f );

		private static void DrawGizmos( BoardComponent[] piles, Color color ) {
			Gizmos.color = color;
			foreach ( var pile in piles ) {
				if ( pile == null ) {
					continue;
				}

				Gizmos.DrawWireCube( pile.transform.position, size );
			}
		}
#endif
		public Vector3 GetWorldPosition( PositionOnBoard position ) {
			var pile = GetPile( position.pile );
			return pile.GetWorldPosition( position.row );
		}

		public Vector3 GetSpacing( PileId pileId ) {
			var pile = GetPile( pileId );
			return pile.spacing;
		}

		private BoardComponent GetPile( PileId pile ) {
			var piles = GetPiles( pile.type );
			return piles[pile.index];
		}

		private BoardComponent[] GetPiles( PileId.Type type ) {
			switch ( type ) {
				case PileId.Type.Free:
					return freeCells;
				case PileId.Type.Home:
					return homeCells;
				case PileId.Type.Table:
					return tablePiles;
				default:
					return null;
			}
		}

		int IBoardLayout.GetNumber( PileId.Type type ) {
			var piles = GetPiles( type );
			if ( piles == null ) {
				return 0;
			}

			return piles.Length;
		}
	}
}