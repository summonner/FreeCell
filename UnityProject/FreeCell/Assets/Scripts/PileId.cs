

namespace Summoner.FreeCell {
	public struct PileId {
		public enum Type {
			Home, Table, Free,
		}

		public readonly Type type;
		public readonly int index;

		public PileId( Type type, int index ) {
			this.type = type;
			this.index = index;
		}

		public static bool operator ==( PileId left, PileId right ) {
			return left.type == right.type
				&& left.index == right.index;
		}

		public static bool operator !=( PileId left, PileId right ) {
			return !(left == right);
		}

		public override bool Equals( object obj ) {
			if ( obj is PileId ) {
				return this == (PileId)obj;
			}

			return base.Equals( obj );
		}

		public override int GetHashCode() {
			return (int)type + index;
		}

		public override string ToString() {
			return type + "[" + index + "]";
		}
	}
}