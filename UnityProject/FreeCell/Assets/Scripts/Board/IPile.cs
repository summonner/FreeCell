using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public interface IPile {
		int Count { get; }
		bool isEmpty { get; }

		void Push( IEnumerable<Card> cards );
		IEnumerable<Card> Pop( int index );
		void Clear();
	}
}