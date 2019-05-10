using UnityEngine;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public class Tableau : IPile {

		private readonly List<Card> stack = new List<Card>( 20 );

		public int Count {
			get {
				return stack.Count;
			}
		}

		public void Push( params Card[] cards ) {
			stack.AddRange( cards );
		}

		public Card[] Pop( int startIndex ) {
			if ( stack.IsOutOfRange( startIndex ) == true ) {
				Debug.Assert( false, "tried to pop too many from pile." );
				return null;
			}

			if ( DoesLinked( startIndex ) == false ) {
				return null;
			}

			var popCount = stack.Count - startIndex;
			var poped = new Card[popCount];
			stack.CopyTo( startIndex, poped, 0, popCount );
			stack.RemoveRange( startIndex, popCount );

			return poped;
		}

		private bool DoesLinked( int index ) {
			for ( int i = stack.Count - 2; i >= index; --i ) {
				if ( DoesLink( stack[i + 1], stack[i] ) == false ) {
					return false;
				}
			}

			return true;
		}

		private bool DoesLink( Card top, Card under ) {

			if ( IsRed( top ) == IsRed( under ) ) {
				return false;
			}

			if ( under.rank - top.rank != 1 ) {
				return false;
			}

			return true;
		}

		public bool IsAcceptable( params Card[] cards ) {
			if ( stack.Count == 0 ) {
				return true;
			}

			var top = stack[stack.Count - 1];
			return DoesLink( cards[0], top );
		}

		private static bool IsRed( Card card ) {
			switch ( card.suit ) {
				case Card.Suit.Diamonds:
				case Card.Suit.Hearts:
					return true;

				case Card.Suit.Spades:
				case Card.Suit.Clubs:
					return false;

				default:
					Debug.Assert( false, "Unknown suit type : " + card );
					return false;
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
	}
}