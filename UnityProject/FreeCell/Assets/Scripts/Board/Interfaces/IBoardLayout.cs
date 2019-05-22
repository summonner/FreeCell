using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public interface IBoardLayout {
		int numHomes { get; }
		int numFrees { get; }
		int numPiles { get; }
	}
}