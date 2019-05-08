using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public struct SelectPosition {
		public readonly PileId pile;
		public readonly int row;

		public PileId.Type type
		{
			get
			{
				return pile.type;
			}
		}

		public int column
		{
			get
			{
				return pile.index;
			}
		}

		public SelectPosition( PileId.Type type, int column, int row )
			: this( new PileId( type, column ), row ) { }

		public SelectPosition( PileId pile, int row ) {
			this.pile = pile;
			this.row = row;
		}

		public override string ToString() {
			return pile.type + "[" + pile.index + ", " + row + "]";
		}
	}
}