using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public class CardTest {

		[Test]
		public void GenerateADeck() {
			var deck = new List<Card>( Card.NewDeck() );
			Assert.AreEqual( 52, deck.Count );

			ulong check = 0ul;
			foreach ( var card in deck ) {
				var index = (int)card.suit * 13 + (int)card.rank - 1;
				check |= 1ul << index;
			}

			Assert.AreEqual( (1ul << 52) - 1, check, check.ToString( "x" ) );
		}
	}
}