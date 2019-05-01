using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public interface IPile {
		int Count { get; }

		void Push( IEnumerable<Card> cards );
		IEnumerable<Card> Pop( int index );
		Card Peek( int index );
		void Clear();
	}
}