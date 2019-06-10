using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public interface IPileLookup : IList<Card> {
		PileId id { get; }

		bool CanMove( int row );
		bool IsAcceptable( params Card[] card );
	}

	public interface IPile : IPileLookup {
		void Push( params Card[] cards );
		Card[] Pop( int startIndex );
	}
}