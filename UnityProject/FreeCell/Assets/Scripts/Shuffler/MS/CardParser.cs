using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public static class CardParser {
		public static readonly string cardPattern = @"([\w\u2660-\u2667\u25c6\u25c7])(\w+)";
		private static readonly Regex regex = new Regex( cardPattern );

		public static IEnumerable<Card> Parse( string text ) {
			var matches = regex.Matches( text );
			foreach ( Match match in matches ) {
				yield return Parse( match.Groups[1], match.Groups[2] );
			}
		}

		public static Card Parse( Group suit, Group rank ) {
			var suitValue = ParseSuit( suit.Value );
			var rankValue = ParseRank( rank.Value );
			return new Card( suitValue, rankValue );
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

				case "":
					return Card.Suit.NONE;

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
				case "T":
				case "t":
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

				case "":
					return Card.Rank.NONE;

				default:
					Debug.LogError( "Unknown rank token : " + rank );
					return Card.Rank.NONE;
			}
		}
	}
}