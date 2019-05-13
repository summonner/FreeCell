using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace Summoner.FreeCell {
	public abstract class BasePile : IPile {
		protected readonly List<Card> stack;
		public readonly PileId id;

		protected BasePile( PileId.Type type, int index, int capacity ) {
			this.id = new PileId( type, index );
			stack = new List<Card>( capacity );
		}

		PileId IPileLookup.id {
			get {
				return this.id;
			}
		}

		public int Count
		{
			get
			{
				return stack.Count;
			}
		}

		public bool IsReadOnly {
			get {
				return true;
			}
		}

		public Card this[int index] {
			get {
				return stack.ElementAtOrDefault( index );
			}
			set {
				throw new System.NotSupportedException();
			}
		}

		public Card LastOrDefault() {
			return stack.LastOrDefault();
		}

		public int IndexOf( Card card ) {
			return stack.IndexOf( card );
		}

		public void Clear() {
			stack.Clear();
		}

		public override string ToString() {
			var str = new System.Text.StringBuilder();
			foreach ( var card in stack ) {
				str.Append( card.ToString() + " " );
			}

			return id.ToString() + " " + str.ToString();
		}

		public abstract void Push( params Card[] cards );
		public abstract Card[] Pop( int index );
		public abstract bool IsAcceptable( params Card[] card );

		void IList<Card>.Insert( int index, Card item ) {
			throw new System.NotSupportedException();
		}

		void IList<Card>.RemoveAt( int index ) {
			throw new System.NotSupportedException();
		}

		void ICollection<Card>.Add( Card item ) {
			throw new System.NotSupportedException();
		}

		bool ICollection<Card>.Contains( Card item ) {
			return stack.Contains( item );
		}

		void ICollection<Card>.CopyTo( Card[] array, int arrayIndex ) {
			throw new System.NotSupportedException();
		}

		bool ICollection<Card>.Remove( Card item ) {
			throw new System.NotSupportedException();
		}

		public IEnumerator<Card> GetEnumerator() {
			return stack.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return stack.GetEnumerator();
		}
	}
}