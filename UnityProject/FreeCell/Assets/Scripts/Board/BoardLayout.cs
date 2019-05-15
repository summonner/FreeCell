using UnityEngine;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public interface IBoardLayout {
		int numHomes { get; }
		int numFrees { get; }
		int numPiles { get; }
	}

	public class BoardLayout : MonoBehaviour, IBoardLayout {
		public Transform[] homeCells;
		public Transform[] freeCells;
		public Transform[] tablePiles;

		public int numPiles = 8;
		public Vector3 spacing = new Vector3( 1.1f, -0.4f, -0.01f );
		public Vector3 offset = new Vector3( 0f, 5f, 0f );

		public int numFrees = 4;
		public const int numHomes = 4;
		public Vector3 headOffset = new Vector3( 0.0f, 1.5f, 0f );

#if UNITY_EDITOR
		[ContextMenu( "Create Init" )]
		void Init() {
			var transform = this.transform;

			Clear();
			var tableOffset = offset;
			tableOffset.x += (numPiles - 1f) * -0.5f * spacing.x;
			tablePiles = BuildPiles( numPiles, tableOffset, "table" );

			var upPartOffset = offset;
			upPartOffset.x += (numFrees + numHomes - 1f + headOffset.x) * -0.5f * spacing.x;
			upPartOffset.y += headOffset.y;
			freeCells = BuildPiles( numFrees, upPartOffset, "free" );

			upPartOffset.x += (numFrees + headOffset.x) * spacing.x;
			homeCells = BuildPiles( numHomes, upPartOffset, "home" );
		}

		private void Clear() {
			while ( transform.childCount > 0 ) {
				DestroyImmediate( transform.GetChild( 0 ).gameObject );
			}
		}

		private Transform[] BuildPiles( int numPiles, Vector3 offset, string name ) {
			var piles = new Transform[numPiles];
			for ( var i = 0; i < numPiles; ++i ) {
				var pos = offset;
				pos.x += i * spacing.x;

				var pile = new GameObject( name + i ).transform;
				pile.parent = transform;
				pile.localPosition = pos;

				piles[i] = pile;
			}

			return piles;
		}

		private void OnDrawGizmos() {
			DrawGizmos( tablePiles, Color.red );
			DrawGizmos( freeCells, Color.cyan );
			DrawGizmos( homeCells, Color.green );
		}

		private static readonly Vector3 size = new Vector3( 1f, 1.357f, 1f );

		private static void DrawGizmos( Transform[] transforms, Color color ) {
			Gizmos.color = color;
			foreach ( var t in transforms ) {
				Gizmos.DrawWireCube( t.position, size );
			}
		}
#endif

		public Transform this[PileId id] {
			get {
				var pile = GetPile( id.type );
				if ( pile.IsOutOfRange( id.index ) == true ) {
					return null;
				}

				return pile[id.index];
			}
		}

		private Transform[] GetPile( PileId.Type type ) {
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