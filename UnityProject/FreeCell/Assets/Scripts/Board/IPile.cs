using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public interface IPile {
		int Count { get; }

		void Push( IList<Card> cards );
		IList<Card> Pop( int index );
		bool IsAcceptable( Card card );
		void Clear();

		IList<Card> GetReadOnly();
	}
}