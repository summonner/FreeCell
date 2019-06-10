using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public interface IBoardLayout {
		int GetNumber( PileId.Type type );
	}
}