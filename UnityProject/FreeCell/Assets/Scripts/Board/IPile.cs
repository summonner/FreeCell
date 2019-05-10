using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public interface IPile {
		int Count { get; }

		void Push( params Card[] cards );
		Card[] Pop( int startIndex );
		bool IsAcceptable( params Card[] card );
		void Clear();

		IList<Card> GetReadOnly();
	}
}