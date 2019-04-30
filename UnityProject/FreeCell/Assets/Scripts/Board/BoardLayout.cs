using UnityEngine;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public class BoardLayout : MonoBehaviour {
		public Transform[] homeCells;
		public Transform[] freeCells;
		public Transform[] piles;

		public int numPiles = 8;
		public Vector3 spacing = new Vector3( 1.1f, -0.4f, -0.01f );
		public Vector3 offset = new Vector3( 0f, 5f, 0f );

#if UNITY_EDITOR
		[ContextMenu( "Create Init" )]
		void Init() {
			var transform = this.transform;

			while ( transform.childCount > 0 ) {
				DestroyImmediate( transform.GetChild( 0 ).gameObject );
			}

			piles = new Transform[numPiles];
			var centerMargin = (numPiles - 1) * 0.5f;
			for ( var i=0; i < numPiles; ++i ) {
				var pos = offset;
				pos.x += (i - centerMargin) * spacing.x;

				var pile = new GameObject( "pile" + i ).transform;
				pile.parent = transform;
				pile.localPosition = pos;

				piles[i] = pile;
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
					return piles;
				default:
					return null;
			}
		}

	}
}