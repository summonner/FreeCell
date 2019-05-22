using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public interface IBoardLookup {
		IPileLookup this[PileId id] { get; }
		IPileLookup this[PileId.Type type, int index] { get; }
		IEnumerable<IPileLookup> this[params PileId.Type[] types] { get; }
	}
}