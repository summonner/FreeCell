using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;

namespace Summoner.FreeCell.Test {
	public class TableauTest {
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
			var pile = new Tableau();
			SinglePush( pile, cards );
			SinglePop( pile, cards );
		}

		[TestCaseSource( "orderedCases" )]
		public void MultiplePushPop( IList<Card> cards ) {
			var pile = new Tableau();
			MultiplePush( pile, cards );
			MultiplePop( pile, cards );
		}

		[TestCaseSource( "wholeCases" )]
		public void MultiplePushSinglePop( IList<Card> cards ) {
			var pile = new Tableau();
			MultiplePush( pile, cards );
			SinglePop( pile, cards );
		}

		[TestCaseSource( "orderedCases" )]
		public void SinglePushMultiplePop( IList<Card> cards ) {
			var pile = new Tableau();
			SinglePush( pile, cards );
			MultiplePop( pile, cards );
		}

		private void SinglePush( Tableau pile, IList<Card> cards ) {
			pile.Clear();
			Assert.AreEqual( 0, pile.Count, "have to initialized as empty" );

			for ( var i = 0; i < cards.Count; ++i ) {
				var card = cards[i];
				pile.Push( new[] { card } );
				Assert.AreEqual( i + 1, pile.Count, "failed to push a card - " + card );
			}
		}

		private void MultiplePush( Tableau pile, IList<Card> cards ) {
			pile.Clear();
			Assert.AreEqual( 0, pile.Count, "have to initialized as empty" );
			pile.Push( cards );
			Assert.AreEqual( cards.Count, pile.Count, "failed to push cards" );
		}

		private void SinglePop( Tableau pile, IList<Card> cards ) {
			for ( var i = pile.Count - 1; i >= 0; --i ) {
				var poped = pile.Pop( i );
				Assert.IsNotNull( poped, "failed to pop a card" );
				Assert.AreEqual( 1, poped.Count, "tried pop single card" );
				Assert.AreEqual( i, pile.Count, "poped card still remained" );
				Assert.AreEqual( cards[i], poped[0], "poped card must be a card that last in " );
			}
		}

		private void MultiplePop( Tableau pile, IList<Card> cards ) {
			var poped = pile.Pop( 0 );
			Assert.IsNotNull( poped, "failed to pop cards" );
			Assert.AreEqual( cards.Count, poped.Count, "tried pop all cards from pile" );
			Assert.AreEqual( 0, pile.Count, "poped cards still remained" );

			CollectionAssert.AreEqual( cards, poped, "poped cards have to have same order with source" );
		}

		private readonly static IEnumerable<Card> deck = Card.NewDeck();
		[TestCaseSource( "deck" )]
		public void AcceptableTest( Card selected ) {
			var pile = new Tableau();

			pile.Push( new [] { selected } );
			int countAcceptables = 0;
			foreach ( var card in deck ) {
				if ( IsSameColor( card.suit, selected.suit ) ) {
					Assert.IsFalse( pile.IsAcceptable( card ), "same colored suit must not acceptable - " + card );
					continue;
				}

				if ( selected.rank - 1 != card.rank ) {
					Assert.IsFalse( pile.IsAcceptable( card ), "can only acceptable the rank is 1 smaller than top of pile - " + card );
					continue;
				}

				Assert.IsTrue( pile.IsAcceptable( card ), "valid card have to acceptable - " + card );
				countAcceptables += 1;
			}

			if ( selected.rank == Card.Rank.Ace ) {
				Assert.AreEqual( 0, countAcceptables, "no cards are acceptable on the next of ace" );
			}
			else {
				Assert.AreEqual( 2, countAcceptables, "must be 2 acceptable cards exist" );
			}
		}

		private static bool IsSameColor( Card.Suit left, Card.Suit right ) {
			IList<Card.Suit> reds = new [] { Card.Suit.Diamonds, Card.Suit.Hearts };
			return reds.Contains( left ) == reds.Contains( right );
		}

		[Test]
		public void AcceptableTestForEmpty() {
			var pile = new Tableau();

			foreach ( var card in deck ) {
				Assert.IsTrue( pile.IsAcceptable( card ), "empty tableau have to accept any cards" );
			}
		}
	}
}