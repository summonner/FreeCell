

namespace Summoner.FreeCell {
	public struct PileId {
		public enum Type {
			Home, Free, Table,
		}

		public readonly Type type;
		public readonly int index;

		public PileId( Type type, int index ) {
			this.type = type;
			this.index = index;
		}
	}
}