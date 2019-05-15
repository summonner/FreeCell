using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public class HomeCell : BasePile {
		public HomeCell( int index )
			: base( PileId.Type.Home, index, 13 ) { }

#if UNITY_EDITOR
		public HomeCell()
			: this( 0 ) { }
#endif

		public override void Push( params Card[] cards ) {
			if ( cards.Length > 1 ) {
				return;
			}

			stack.AddRange( cards );
		}

		public override Card[] Pop( int startIndex ) {
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

		public override bool IsAcceptable( params Card[] cards ) {
			if ( cards.Length != 1 ) {
				return false;
			}

			return IsValid( stack.LastOrDefault(), cards[0] );
		}

		public static bool IsValid( Card first, Card next ) {
			if ( first == Card.Blank ) {
				return next.rank == Card.Rank.Ace;
			}

			return first.suit == next.suit
				&& (first.rank + 1) == next.rank;
		}
	}
}