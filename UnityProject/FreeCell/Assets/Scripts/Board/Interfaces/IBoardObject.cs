using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public interface IBoardObject {
		PositionOnBoard position { get; }
	}
}