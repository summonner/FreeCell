using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public class HomeCell : IPile {

		private readonly List<Card> stack = new List<Card>( 13 );

		public int Count
		{
			get
			{
				return stack.Count;
			}
		}

		public void Push( IList<Card> cards ) {
			if ( cards.Count > 1 ) {
				return;
			}

			stack.AddRange( cards );
		}

		public IList<Card> Pop( int startIndex ) {
			if ( stack.IsOutOfRange( startIndex ) == true ) {
				Debug.Assert( false, "tried to pop too many from pile." );
				return null;
			}

			var popCount = stack.Count - startIndex;
			var poped = new Card[popCount];
			stack.CopyTo( startIndex, poped, 0, popCount );
			stack.RemoveRange( startIndex, popCount );

			return poped;
		}

		public bool IsAcceptable( Card card ) {
			if ( stack.Count <= 0 ) {
				return card.rank == Card.Rank.Ace;
			}

			var top = stack.Last();
			if ( card.suit != top.suit ) {
				return false;
			}

			return card.rank == (top.rank + 1);
		}

		public void Clear() {
			stack.Clear();
		}

		public IList<Card> GetReadOnly() {
			return stack.AsReadOnly();
		}
	}
}