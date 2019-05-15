using UnityEngine;

using System.Collections.Generic;
using System.Text.RegularExpressions;

using Summoner.Util.Extension;

namespace Summoner.FreeCell.Test {
	public class TestCardList {
		public readonly IList<Card> results = null;
		public readonly int selectIndex = -1;

		public int Count {
			get {
				return results.Count;
			}
		}

		public Card this[int index] {
			get {
				return results[index];
			}
		}

		private static readonly Regex regex = new Regex( @"(\*?)([\w\u2660-\u2667\u25c6\u25c7])(\w+)|_" );

		public TestCardList( string pile ) {
			var matches = regex.Matches( pile );
			var results = new Card[matches.Count];

			for ( int i=0; i < matches.Count; ++i ) {
				var tokens = matches[i].Groups;

				results[i] = ParseCard( tokens );

				var isTarget = ParseOperation( tokens[1].Value );
				if ( isTarget == true ) {
					selectIndex = i;
				}
			}

			this.results = results.AsReadOnly();
		}

		private static Card ParseCard( GroupCollection tokens ) {
			if ( tokens[0].Value == "_" ) {
				return Card.Blank;
			}

			var suit = ParseSuit( tokens[2].Value );
			var rank = ParseRank( tokens[3].Value );
			return new Card( suit, rank );
		}

		private static bool ParseOperation( string token ) {
			return token == "*";
		}

		private static Card.Suit ParseSuit( string suit ) {
			switch ( suit ) {
				case "S":
				case "s":
				case "♠":
				case "♤":
					return Card.Suit.Spades;

				case "H":
				case "h":
				case "♡":
				case "♥":
					return Card.Suit.Hearts;

				case "D":
				case "d":
				case "♢":
				case "♦":
				case "◇":
				case "◆":
					return Card.Suit.Diamonds;

				case "C":
				case "c":
				case "♣":
				case "♧":
					return Card.Suit.Clubs;

				default:
					Debug.LogError( "Unknown suit token : " + suit );
					return Card.Suit.NONE;
			}
		}

		private static Card.Rank ParseRank( string rank ) {
			switch ( rank ) {
				case "A":
				case "a":
				case "1":
					return Card.Rank.Ace;

				case "2":
					return Card.Rank._2;

				case "3":
					return Card.Rank._3;

				case "4":
					return Card.Rank._4;

				case "5":
					return Card.Rank._5;

				case "6":
					return Card.Rank._6;

				case "7":
					return Card.Rank._7;

				case "8":
					return Card.Rank._8;

				case "9":
					return Card.Rank._9;

				case "0":
				case "10":
					return Card.Rank._10;

				case "J":
				case "j":
				case "11":
					return Card.Rank.Jack;

				case "Q":
				case "q":
				case "12":
					return Card.Rank.Queen;

				case "K":
				case "k":
				case "13":
					return Card.Rank.King;

				default:
					Debug.LogError( "Unknown rank token : " + rank );
					return Card.Rank.NONE;
			}
		}
	}
}