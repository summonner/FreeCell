using System.Collections.Generic;

namespace Summoner.FreeCell {
	public struct Card {
		public enum Suit {
			Spades = 0, Hearts, Diamonds, Clubs
		}

		public enum Rank {
			Ace = 1, _2, _3, _4, _5, _6, _7, _8, _9, _10, Jack, Queen, King
		}

		public readonly Suit suit;
		public readonly Rank rank;
		
		public Card( Suit suit, Rank rank ) {
			this.suit = suit;
			this.rank = rank;
		}

		public static IEnumerable<Card> NewDeck() {
			var suits = new [] { Suit.Spades, Suit.Hearts, Suit.Diamonds, Suit.Clubs };
			var ranks = new [] { Rank.Ace, Rank._2, Rank._3, Rank._4, Rank._5,
								 Rank._6, Rank._7, Rank._8, Rank._9, Rank._10,
								 Rank.Jack, Rank.Queen, Rank.King };

			foreach ( var suit in suits ) {
				foreach ( var rank in ranks ) {
					yield return new Card( suit, rank );
				}
			}
		}

		public override string ToString() {
			return ToString( suit ) + ToString( rank );
		}

		private static string ToString( Suit suit ) {
			switch ( suit ) {
				case Suit.Spades:	return "♠";
				case Suit.Hearts:	return "♡";
				case Suit.Diamonds:	return "◇";
				case Suit.Clubs:	return "♣";
				default:			return " ";
			}
		}

		private static string ToString( Rank rank ) {
			switch ( rank ) {
				case Rank.Ace:		return "A";
				case Rank._2:		return "2";
				case Rank._3:		return "3";
				case Rank._4:		return "4";
				case Rank._5:		return "5";
				case Rank._6:		return "6";
				case Rank._7:		return "7";
				case Rank._8:		return "8";
				case Rank._9:		return "9";
				case Rank._10:		return "0";
				case Rank.Jack:		return "J";
				case Rank.Queen:	return "Q";
				case Rank.King:		return "K";
				default:			return " ";
			}
		}
	}
}
