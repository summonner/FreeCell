using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public class Pile {

		private List<Card> stack = new List<Card>( 20 );

		public int Count {
			get {
				return stack.Count;
			}
		}

		public bool isEmpty {
			get {
				return stack.Count <= 0;
			}
		}

		public void Push( params Card[] cards ) {
			Push( (IEnumerable<Card>)cards );
		}

		public void Push( IEnumerable<Card> card ) {
			stack.AddRange( card );
		}

		public IList<Card> Pop( int count ) {
			if ( count > stack.Count ) {
				Debug.Assert( false, "tried to pop too many from pile." );
				return null;
			}

			var popIndex = stack.Count - count;
			var poped = new Card[count];
			stack.CopyTo( popIndex, poped, 0, count );
			stack.RemoveRange( popIndex, count );

			return poped;
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