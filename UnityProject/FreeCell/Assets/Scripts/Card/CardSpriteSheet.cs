using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Summoner.FreeCell {
	public class CardSpriteSheet : ScriptableObject {
		[SerializeField] private Sprite[] cards;

		public Sprite this[Card card] {
			get {
				var index = (int)card.suit * 13 + (int)card.rank;
				return cards[index];
			}
		}
	}
}