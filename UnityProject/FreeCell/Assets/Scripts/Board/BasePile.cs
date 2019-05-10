using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public abstract class BasePile : IPile {
		protected readonly List<Card> stack;

		protected BasePile( int capacity ) {
			stack = new List<Card>( capacity );
		}

		public int Count
		{
			get
			{
				return stack.Count;
			}
		}

		public void Clear() {
			stack.Clear();
		}

		public IList<Card> GetReadOnly() {
			return stack.AsReadOnly();
		}

		public override string ToString() {
			var str = new System.Text.StringBuilder();
			foreach ( var card in stack ) {
				str.Append( card.ToString() + " " );
			}

			return str.ToString();
		}

		public abstract void Push( params Card[] cards );
		public abstract Card[] Pop( int index );
		public abstract bool IsAcceptable( params Card[] card );
	}
}