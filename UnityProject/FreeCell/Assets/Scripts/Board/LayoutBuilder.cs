using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public class LayoutBuilder : MonoBehaviour {
		public BoardLayout layout;
		private new Transform transform;

		public int numPiles = 8;
		public Vector3 spacing = new Vector3( 1.1f, -0.4f, -0.01f );
		public Vector3 offset = new Vector3( 0f, 5f, 0f );

		public int numFrees = 4;
		public const int numHomes = 4;
		public Vector3 headOffset = new Vector3( 0.1f, 1.5f, 0f );

		public Vector2 cardSize = new Vector2( 1.05f, 1.357f );

#if UNITY_EDITOR
		[ContextMenu( "Build" )]
		void Init() {
			if ( layout == null ) {
				Debug.LogWarning( "no BoardLayout assigned" );
				return;
			}

			transform = layout.transform;

			var tableOffset = offset;
			tableOffset.x += (numPiles - 1f) * -0.5f * spacing.x;
			layout.tablePiles = BuildPiles( numPiles, tableOffset, PileId.Type.Table );

			var upPartOffset = offset;
			upPartOffset.x += (numFrees + numHomes - 1f + headOffset.x) * -0.5f * spacing.x;
			upPartOffset.y += headOffset.y;
			layout.freeCells = BuildPiles( numFrees, upPartOffset, PileId.Type.Free );

			upPartOffset.x += (numFrees + headOffset.x) * spacing.x;
			layout.homeCells = BuildPiles( numHomes, upPartOffset, PileId.Type.Home );
		}

		private BoardComponent[] BuildPiles( int numPiles, Vector3 offset, PileId.Type type ) {
			var piles = new BoardComponent[numPiles];
			for ( var i = 0; i < numPiles; ++i ) {
				var pos = offset;
				pos.x += i * spacing.x;

				var pile = GetTransform( type.ToString() + i );
				
				SetPosition( pile, transform, pos );
				SetLayer( pile );
				AddCollider( pile, cardSize );
				var script = AddScript( pile, type, i, spacing );

				piles[i] = script;
			}

			return piles;
		}

		private Transform GetTransform( string name ) {
			var child = transform.Find( name );
			if ( child != null ) {
				return child;
			}

			return new GameObject( name ).transform;
		}

		private static void SetPosition( Transform pile, Transform parent, Vector3 localPosition ) {
			pile.parent = parent;
			pile.localPosition = localPosition;
		}

		private static void SetLayer( Transform pile ) {
			pile.gameObject.layer = LayerMask.NameToLayer( "Board" );
		}

		private static void AddCollider( Transform pile, Vector2 cardSize ) {
			var collider = pile.GetOrAddComponent<BoxCollider2D>();
			collider.size = cardSize;
		}

		private static BoardComponent AddScript( Transform pile, PileId.Type type, int column, Vector3 spacing ) {
			var script = pile.GetOrAddComponent<BoardComponent>();
			script.type = type;
			script.column = column;
			script.spacing = CalculateSpacing( spacing, type );
			return script;
		}
		
		private static Vector3 CalculateSpacing( Vector3 spacing, PileId.Type type ) {
			spacing.x = 0f;
			if ( type != PileId.Type.Table ) {
				spacing.y = 0f;
			}
			return spacing;
		}
#endif
	}
}