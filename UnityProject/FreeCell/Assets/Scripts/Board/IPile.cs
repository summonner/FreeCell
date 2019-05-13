using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public interface IPileLookup : IList<Card> {
		PileId id { get; }
	}

	public interface IPile : IPileLookup {
		void Push( params Card[] cards );
		Card[] Pop( int startIndex );
		bool IsAcceptable( params Card[] card );
	}
}