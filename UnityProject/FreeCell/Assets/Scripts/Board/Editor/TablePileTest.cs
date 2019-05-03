using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public class TablePileTest {
		#region testCases
		public static IList<Card>[] unorderedCases = new [] {
			new [] {
				new Card( Card.Suit.Spades, Card.Rank.Ace ),
				new Card( Card.Suit.Spades, Card.Rank._2 ),
				new Card( Card.Suit.Spades, Card.Rank._3 ),
				new Card( Card.Suit.Spades, Card.Rank._4 ),
			},
			new [] {
				new Card( Card.Suit.Hearts, Card.Rank.Ace ),
				new Card( Card.Suit.Diamonds, Card.Rank._2 ),
				new Card( Card.Suit.Clubs, Card.Rank._3 ),
				new Card( Card.Suit.Spades, Card.Rank.King ),
			},
		};

		public static IList<Card>[] orderedCases = new [] {
			new [] {
				new Card( Card.Suit.Spades, Card.Rank.King ),
				new Card( Card.Suit.Hearts, Card.Rank.Queen ),
				new Card( Card.Suit.Clubs, Card.Rank.Jack ),
				new Card( Card.Suit.Diamonds, Card.Rank._10 ),
			},
			new [] {
				new Card( Card.Suit.Diamonds, Card.Rank._7 ),
				new Card( Card.Suit.Clubs, Card.Rank._6 ),
				new Card( Card.Suit.Diamonds, Card.Rank._5 ),
				new Card( Card.Suit.Clubs, Card.Rank._4 ),
			}
		};

		public static IEnumerable<IList<Card>> wholeCases {
			get {
				foreach ( var aCase in unorderedCases ) {
					yield return aCase;
				}
				foreach ( var aCase in orderedCases ) {
					yield return aCase;
				}
			}
		}
		#endregion

		[TestCaseSource( "wholeCases" )]
		public void SinglePushPop( IList<Card> cards ) {
			var pile = new TablePile();
			SinglePush( pile, cards );
			SinglePop( pile, cards );
		}

		[TestCaseSource( "orderedCases" )]
		public void MultiplePushPop( IList<Card> cards ) {
			var pile = new TablePile();
			MultiplePush( pile, cards );
			MultiplePop( pile, cards );
		}

		[TestCaseSource( "wholeCases" )]
		public void MultiplePushSinglePop( IList<Card> cards ) {
			var pile = new TablePile();
			MultiplePush( pile, cards );
			SinglePop( pile, cards );
		}

		[TestCaseSource( "orderedCases" )]
		public void SinglePushMultiplePop( IList<Card> cards ) {
			var pile = new TablePile();
			SinglePush( pile, cards );
			MultiplePop( pile, cards );
		}

		private void SinglePush( TablePile pile, IList<Card> cards ) {
			pile.Clear();
			Assert.AreEqual( 0, pile.Count, "have to initialized as empty" );

			for ( var i = 0; i < cards.Count; ++i ) {
				var card = cards[i];
				pile.Push( new[] { card } );
				Assert.AreEqual( i + 1, pile.Count, "failed to push a card - " + card );
			}
		}

		private void MultiplePush( TablePile pile, IList<Card> cards ) {
			pile.Clear();
			Assert.AreEqual( 0, pile.Count, "have to initialized as empty" );
			pile.Push( cards );
			Assert.AreEqual( cards.Count, pile.Count, "failed to push cards" );
		}

		private void SinglePop( TablePile pile, IList<Card> cards ) {
			for ( var i = pile.Count - 1; i >= 0; --i ) {
				var poped = pile.Pop( i );
				Assert.IsNotNull( poped, "failed to pop a card" );
				Assert.AreEqual( 1, poped.Count, "tried pop single card" );
				Assert.AreEqual( i, pile.Count, "poped card still remained" );
				Assert.AreEqual( cards[i], poped[0], "poped card must be a card that last in " );
			}
		}

		private void MultiplePop( TablePile pile, IList<Card> cards ) {
			var poped = pile.Pop( 0 );
			Assert.IsNotNull( poped, "failed to pop cards" );
			Assert.AreEqual( cards.Count, poped.Count, "tried pop all cards from pile" );
			Assert.AreEqual( 0, pile.Count, "poped cards still remained" );

			CollectionAssert.AreEqual( cards, poped, "poped cards have to have same order with source" );
		}

		[Test]
		public void RuleTest() {
			var pile = new TablePile();

			var diamond10 = new Card( Card.Suit.Diamonds, Card.Rank._10 );
			var club9 = new Card( Card.Suit.Clubs, Card.Rank._9 );
			var spade9 = new Card( Card.Suit.Spades, Card.Rank._9 );
			var diamond9 = new Card( Card.Suit.Diamonds, Card.Rank._9 );
			var heart9 = new Card( Card.Suit.Hearts, Card.Rank._9 );
			var club8 = new Card( Card.Suit.Clubs, Card.Rank._8 );
			var spade8 = new Card( Card.Suit.Spades, Card.Rank._8 );
			var diamond8 = new Card( Card.Suit.Diamonds, Card.Rank._8 );
			var heart8 = new Card( Card.Suit.Hearts, Card.Rank._8 );

			pile.Push( new [] { diamond10 } );
			Assert.IsTrue( pile.IsAcceptable( club9 ), club9 + " have to acceptable" );
			Assert.IsTrue( pile.IsAcceptable( spade9 ), spade9 + " have to acceptable" );
			Assert.IsFalse( pile.IsAcceptable( diamond9 ), diamond9 + " must not acceptable" );
			Assert.IsFalse( pile.IsAcceptable( heart9 ), heart9 + " must not acceptable" );
			Assert.IsFalse( pile.IsAcceptable( club8 ), club8 + " must not acceptable" );
			Assert.IsFalse( pile.IsAcceptable( spade8 ), spade8 + " must not acceptable" );
			Assert.IsFalse( pile.IsAcceptable( diamond8 ), diamond8 + " must not acceptable" );
			Assert.IsFalse( pile.IsAcceptable( heart8 ), heart8 + " must not acceptable" );
		}
	}
}