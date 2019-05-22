using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public interface IBoardController {
		IPile this[PileId pile] { get; }
		IPile this[PileId.Type type, int index] { get; }
		IEnumerable<IPile> this[params PileId.Type[] type] { get; }
		int CountMaxMovables();
	}
}