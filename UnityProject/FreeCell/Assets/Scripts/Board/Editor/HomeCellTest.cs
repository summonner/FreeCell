using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

namespace Summoner.FreeCell.Test {
	public class HomeCellTest {

		[Test]
		public void OverrallTest() {
			var spadeAce = new Card( Card.Suit.Spades, Card.Rank.Ace );
			var spade2 = new Card( Card.Suit.Spades, Card.Rank._2 );

			var cell = new HomeCell();
			Assert.AreEqual( 0, cell.Count, "must be empty after initialize." );

			Assert.IsTrue( cell.IsAcceptable( spadeAce ), "empty home cell have to accept any ace card" );
			Assert.IsFalse( cell.IsAcceptable( spade2 ), "empty home cell must not accept any non ace cards" );

			cell.Push( spadeAce );
			Assert.AreEqual( 1, cell.Count, "failed to push a card" );

			Assert.IsFalse( cell.IsAcceptable( spadeAce ), "all cards except spade2 have to discard" );
			Assert.IsTrue( cell.IsAcceptable( spade2 ), "only spade2 can accept" );

			cell.Push( spade2 );
			Assert.AreEqual( 2, cell.Count, "failed to push a card" );

			var poped = cell.Pop( 1 );
			Assert.IsNotNull( poped, "pop function need to available for undo function" );
			Assert.AreEqual( 1, poped.Count, "tried to pop a single card" );
			Assert.AreEqual( spade2, poped[0], "poped card must be a card that last in" );
		}

		[Test]
		public void WholeDeckTest() {
			var cells = new [] {
				new HomeCell(),
				new HomeCell(),
				new HomeCell(),
				new HomeCell(),
			};

			var shuffledDeck = Util.Random.FisherYatesShuffle.Shuffle( Card.NewDeck() );

			while( shuffledDeck.Count > 0 ) {
				for ( int i = shuffledDeck.Count - 1; i >= 0; --i ) {
					var card = shuffledDeck[i];
					foreach ( var cell in cells ) {
						if ( cell.IsAcceptable( card ) == true ) {
							cell.Push( card );
							shuffledDeck.Remove( card );
							break;
						}
					}
				}
			}

			foreach ( var cell in cells ) {
				Assert.AreEqual( 13, cell.Count, "each cells have to have 13 cards" );
				var cards = cell.Pop( 0 );
				var suit = cards[0].suit;
				for ( int i=0; i < cards.Count; ++i ) {
					Assert.AreEqual( suit, cards[i].suit, "all cards of each cells must be same suit" );
					Assert.AreEqual( (Card.Rank)(i + 1), cards[i].rank, "cards must be sorted in ascending order" );
				}
			}
		}
	}
}