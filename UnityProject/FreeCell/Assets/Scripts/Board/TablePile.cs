using UnityEngine;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public class TablePile : IPile {

		private List<Card> stack = new List<Card>( 20 );

		public int Count {
			get {
				return stack.Count;
			}
		}

		public void Push( IEnumerable<Card> card ) {
			stack.AddRange( card );
		}

		public IEnumerable<Card> Pop( int index ) {
			if ( stack.IsOutOfRange( index ) == true ) {
				Debug.Assert( false, "tried to pop too many from pile." );
				return null;
			}

			var popCount = stack.Count - index;
			var poped = new Card[popCount];
			stack.CopyTo( index, poped, 0, popCount );
			stack.RemoveRange( index, popCount );

			return poped;
		}

		public Card Peek( int index ) {
			Debug.Assert( stack.IsOutOfRange( index ) == false, "tried to access the invalid index" );
			return stack[index];
		}

		public void Clear() {
			stack.Clear();
		}

		public override string ToString() {
			var str = new System.Text.StringBuilder();
			foreach ( var card in stack ) {
				str.Append( card.ToString() + " " );
			}

			return str.ToString();
		}
	}
}