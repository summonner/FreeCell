using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public interface IBoardPreset {
		IEnumerable<Card> homes { get; }
		IEnumerable<Card> frees { get; }
		IEnumerable<Card> tableau { get; }
	}
}