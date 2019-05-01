using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Summoner.FreeCell {
	public class CardSpriteSheet : ScriptableObject {
		[SerializeField] private Sprite[] cards;
		[SerializeField] private CardObject template;

		public Sprite this[Card card] {
			get {
				var index = FindIndex( card );
				return cards[index];
			}
		}

		private int FindIndex( Card card ) {
			return (int)card.suit * 13 + (int)card.rank;
		}

		public CardObject NewObject( Card card ) {
			var cardObject = Instantiate( template );
			cardObject.sprite = this[card];
			return cardObject;
		}
	}
}