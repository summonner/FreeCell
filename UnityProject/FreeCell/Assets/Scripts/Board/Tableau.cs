using UnityEngine;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public class Tableau : BasePile {
		public Tableau( int index )
			: base( PileId.Type.Table, index, 20 ) { }

#if UNITY_EDITOR
		public Tableau()
			: this( 0 ) { }
#endif

		public override void Push( params Card[] cards ) {
			stack.AddRange( cards );
		}

		public override Card[] Pop( int startIndex ) {
			if ( CanMove( startIndex ) == false ) {
				return null;
			}

			var popCount = stack.Count - startIndex;
			var poped = new Card[popCount];
			stack.CopyTo( startIndex, poped, 0, popCount );
			stack.RemoveRange( startIndex, popCount );

			return poped;
		}

		public override bool CanMove( int startIndex ) {
			if ( stack.IsOutOfRange( startIndex ) == true ) {
				Debug.Assert( false, "tried to pop too many from pile." );
				return false;
			}

			if ( DoesLinked( startIndex ) == false ) {
				return false;
			}

			return true;
		}

		private bool DoesLinked( int index ) {
			for ( int i = stack.Count - 2; i >= index; --i ) {
				if ( IsStackable( stack[i + 1], stack[i] ) == false ) {
					return false;
				}
			}

			return true;
		}

		public static bool IsStackable( Card top, Card under ) {
			if ( AreDifferentColor( top, under ) == false ) {
				return false;
			}

			if ( under.rank - top.rank != 1 ) {
				return false;
			}

			return true;
		}

		public override bool IsAcceptable( params Card[] cards ) {
			if ( stack.Count == 0 ) {
				return true;
			}

			var top = stack[stack.Count - 1];
			return IsStackable( cards[0], top );
		}

		private static bool AreDifferentColor( Card left, Card right ) {
			return GetColor( left ) + GetColor( right ) == 0;
		}

		private static int GetColor( Card card ) {
			switch ( card.suit ) {
				case Card.Suit.Diamonds:
				case Card.Suit.Hearts:
					return 1;

				case Card.Suit.Spades:
				case Card.Suit.Clubs:
					return -1;

				default:
					return 3;
			}
		}
	}
}