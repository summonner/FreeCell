using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public class BoardComponent : MonoBehaviour, IBoardObject {
		[Readonly] public PileId.Type type;
		[Readonly] public int column;
		public Vector3 spacing = Vector3.zero;

		private new Transform transform;
		public PositionOnBoard position { get; private set; }

		void Awake() {
			transform = base.transform;
			position = new PositionOnBoard( type, column, -1 );
		}

		public Vector3 GetWorldPosition( int row ) {
			return transform.position + spacing * row;
		}
	}
}