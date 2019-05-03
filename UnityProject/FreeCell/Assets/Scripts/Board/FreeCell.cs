using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public class FreeCell : IPile {

		private readonly List<Card> stack = new List<Card>( 1 );

		public int Count
		{
			get
			{
				return stack.Count;
			}
		}

		private bool isEmpty {
			get {
				return stack.Count <= 0;
			}
		}

		public void Push( IList<Card> cards ) {
			if ( stack.Count > 0 ) {
				return;
			}

			stack.Add( cards[0] );
		}

		public IList<Card> Pop( int index ) {
			if ( isEmpty == true ) {
				return null;
			}

			Debug.Assert( stack.Count == 1 );
			var poped = stack.ToArray();
			stack.Clear();

			return poped;
		}

		public bool IsAcceptable( Card card ) {
			return isEmpty;
		}

		public void Clear() {
			stack.Clear();
		}

		public IList<Card> GetReadOnly() {
			return stack.AsReadOnly();
		}
	}
}