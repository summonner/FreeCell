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
			var piles = GetPile( position.type );
			var pile = piles.ElementAt( position.column );
			return pile.GetWorldPosition( position.row );
		}

		private BoardComponent[] GetPile( PileId.Type type ) {
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


		int IBoardLayout.numHomes {
			get {
				return homeCells.Length;
			}
		}

		int IBoardLayout.numFrees {
			get {
				return freeCells.Length;
			}
		}

		int IBoardLayout.numPiles {
			get {
				return tablePiles.Length;
			}
		}
	}
}