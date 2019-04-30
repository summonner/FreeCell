using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public class BoardLayout : MonoBehaviour {
		public Transform[] homeCells;
		public Transform[] freeCells;
		public Transform[] piles;

		public int numPiles = 8;
		public Vector3 spacing = new Vector3( 1.1f, 0.25f, 0.01f );
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
	}
}