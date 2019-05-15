using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public interface IBoardPreset {
		IEnumerable<Card> homes { get; }
		IEnumerable<Card> frees { get; }
		IEnumerable<Card> tableau { get; }
	}

	public class BasicPlacement : IBoardPreset {
		public IEnumerable<Card> homes {
			get	{
				yield break;
			}
		}

		public IEnumerable<Card> frees {
			get {
				yield break;
			}
		}

		public IEnumerable<Card> tableau {
			get {
				return cards;
			}
		}

		private readonly List<Card> cards;
		public BasicPlacement( IEnumerable<Card> deck, int seed ) {
			var random = new System.Random( seed );
			cards = Util.Random.FisherYatesShuffle.Shuffle( deck, random.Next );
		}
	}
}