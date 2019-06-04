using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public struct PositionOnBoard {
		public readonly PileId pile;
		public readonly int row;

		public PileId.Type type	{
			get	{
				return pile.type;
			}
		}

		public int column {
			get	{
				return pile.index;
			}
		}

		public PositionOnBoard( PileId.Type type, int column, int row )
			: this( new PileId( type, column ), row ) { }

		public PositionOnBoard( PileId pile, int row ) {
			this.pile = pile;
			this.row = row;
		}

		public override string ToString() {
			return pile.type + "[" + pile.index + ", " + row + "]";
		}

		public static bool operator == ( PositionOnBoard left, PositionOnBoard right ) {
			return left.pile == right.pile
				&& left.row == right.row;
		}

		public static bool operator != ( PositionOnBoard left, PositionOnBoard right ) {
			return !(left == right);
		}

		public override bool Equals( object obj ) {
			if ( obj is PositionOnBoard ) {
				return this == (PositionOnBoard)obj;
			}

			return base.Equals( obj );
		}

		public override int GetHashCode() {
			return pile.GetHashCode() + row;
		}
	}
}