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
				var index = (int)card.suit * 13 + (int)card.rank;
				check |= 1ul << index;
			}

			Assert.AreEqual( (1ul << 52) - 1, check, check.ToString( "x" ) );
		}

		[Test]
		public void EqualTest() {
			var spadeAce = new Card( Card.Suit.Spades, Card.Rank.Ace );
			var anotherSpadeAce = new Card( Card.Suit.Spades, Card.Rank.Ace );

			Assert.AreEqual( spadeAce, spadeAce, "failed to equal test between same object" );
			Assert.AreEqual( spadeAce, anotherSpadeAce, "failed to equal test between other object" );
		}

		[Test]
		public void NotEqualTest() {
			var spadeAce = new Card( Card.Suit.Spades, Card.Rank.Ace );
			var spade2 = new Card( Card.Suit.Spades, Card.Rank._2 );

			Assert.AreNotEqual( spadeAce, spade2, "failed to not equal test" );
		}
	}
}