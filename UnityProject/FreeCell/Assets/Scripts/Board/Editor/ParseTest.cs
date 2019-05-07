using UnityEngine;
using UnityEditor;
using NUnit.Framework;

using System.Text;
using System.Collections.Generic;

using Suit = System.Collections.Generic.KeyValuePair<Summoner.FreeCell.Card.Suit, string[]>;
using Rank = System.Collections.Generic.KeyValuePair<Summoner.FreeCell.Card.Rank, string[]>;

namespace Summoner.FreeCell.Test {
	[TestOf( typeof( TestCardList ) )]
	public class ParseTest {

		[Test, Pairwise]
		public void Cards(
			[ValueSource("suits")] Suit suits, 
			[ValueSource("ranks")] Rank ranks )
		{
			StringBuilder str = new StringBuilder();
			foreach ( var suit in suits.Value ) {
				foreach ( var rank in ranks.Value ) {
					str.Append( suit );
					str.Append( rank );
					str.Append( " " );
				}
			}

			var text = str.ToString();
			var parse = new TestCardList( text );
			Assert.AreEqual( ranks.Value.Length * suits.Value.Length, parse.results.Count, "not enough results" );

			var expected = new Card( suits.Key, ranks.Key );
			for ( int i=0; i < parse.results.Count; ++i ) {
				Assert.AreEqual( expected, parse.results[i], "current index : " + i + "\nsource text : " + text );
			}
		}

		[Test]
		public void Blanks() {
			var parse = new TestCardList( "_ _ _ _" );
			Assert.AreEqual( 4, parse.results.Count, "not enough results" );
			foreach ( var current in parse.results ) {
				Assert.AreEqual( Card.Blank, current, "must be blank" );
			}
		}

		[Test]
		public void Empty() {
			var parse = new TestCardList( "" );
			Assert.AreEqual( 0, parse.results.Count, "expected no cards" );
		}

		[TestCaseSource( "selectCases" )]
		public int Select( string text ) {
			return new TestCardList( text ).targetIndex;
		}

		public static IEnumerable<TestCaseData> selectCases {
			get {
				yield return new TestCaseData( "D1 S3 CK *H8" ).Returns( 3 );
				yield return new TestCaseData( "h3 c9 *hK c2" ).Returns( 2 );
				yield return new TestCaseData( "*s0 s9 c3 d8" ).Returns( 0 );	
			}
		}

#pragma warning disable 0414
		private static IEnumerable<Suit> suits = new[] {
			new Suit( Card.Suit.Spades, new [] { "s", "S", "♠", "♤" } ),
			new Suit( Card.Suit.Hearts, new [] { "h", "H", "♡", "♥" } ),
			new Suit( Card.Suit.Diamonds, new [] { "d", "D", "◇", "◆", "♢", "♦" } ),
			new Suit( Card.Suit.Clubs, new [] { "c", "C", "♣", "♧" } ),
		};

		private static IEnumerable<Rank> ranks = new [] {
			new Rank( Card.Rank.Ace, new [] { "a", "A", "1" } ),
			new Rank( Card.Rank._2, new [] { "2" } ),
			new Rank( Card.Rank._3, new [] { "3" } ),
			new Rank( Card.Rank._4, new [] { "4" } ),
			new Rank( Card.Rank._5, new [] { "5" } ),
			new Rank( Card.Rank._6, new [] { "6" } ),
			new Rank( Card.Rank._7, new [] { "7" } ),
			new Rank( Card.Rank._8, new [] { "8" } ),
			new Rank( Card.Rank._9, new [] { "9" } ),
			new Rank( Card.Rank._10, new [] { "0", "10" } ),
			new Rank( Card.Rank.Jack, new [] { "j", "J", "11" } ),
			new Rank( Card.Rank.Queen, new [] { "q", "Q", "12" } ),
			new Rank( Card.Rank.King, new [] { "k", "K", "13" } ),
		};
#pragma warning restore 0414
	}
}