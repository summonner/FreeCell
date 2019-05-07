using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;

namespace Summoner.FreeCell.Test {
	public class FreeCellTest {
		private static readonly Card spadeAce = new Card( Card.Suit.Spades, Card.Rank.Ace );
		private static readonly Card spade2 = new Card( Card.Suit.Spades, Card.Rank._2 );

		[Test]
		public void OverrallTest() {
			var cell = new FreeCell();
			Assert.AreEqual( 0, cell.Count, "have to initialized as empty" );

			Assert.IsTrue( cell.IsAcceptable( spadeAce ), "empty free cell have to accept any card" );
			Assert.IsTrue( cell.IsAcceptable( spade2 ), "empty free cell have to accept any card" );
			cell.Push( spade2 );
			Assert.AreEqual( 1, cell.Count, "failed to push a card" );

			Assert.IsFalse( cell.IsAcceptable( spadeAce ), "filled cell must not accept any cards" );
			Assert.IsFalse( cell.IsAcceptable( spade2 ), "filled cell must not accept any cards" );
			cell.Push( spadeAce );
			Assert.AreEqual( 1, cell.Count, "must fail to push a card to filled cell" );

			var poped = cell.Pop( 0 );
			Assert.IsNotNull( poped, "failed to pop a card" );
			Assert.AreEqual( 1, poped.Count, "must be there is a single card" );
			Assert.AreEqual( spade2, poped[0], "poped card must be first one" );
			Assert.AreEqual( 0, cell.Count, "a card still remained after pop operation" );
		}

		[Test]
		public void WholeDeckTest() {
			var shuffledDeck = Util.Random.FisherYatesShuffle.Shuffle( Card.NewDeck() );
			var cell = new FreeCell();

			foreach ( var card in shuffledDeck ) {
				Assert.IsTrue( cell.IsAcceptable( card ), card + " have discard from emtpy free cell" );

				cell.Push( card );
				Assert.AreEqual( 1, cell.Count, "failed to push a card - " + card );

				cell.Pop( 0 );
			}

			cell.Push( shuffledDeck[0] );
			foreach ( var card in shuffledDeck ) {
				Assert.IsFalse( cell.IsAcceptable( card ), card + " have accepted from filled free cell" );
			}
		}
	}
}