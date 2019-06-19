using System.Linq;
using System.Collections.Generic;
using Summoner.Util.Random;

// ref : https://rosettacode.org/wiki/Deal_cards_for_FreeCell

namespace Summoner.FreeCell {
	public class MSShuffler : IBoardPreset {
		private readonly IList<Card> cards;
		private readonly MSRandom random;
		public MSShuffler( int seed ) {
			random = new MSRandom( seed );
			cards = FisherYatesShuffle.Draw( map, random.Range ).ToArray();
		}

		public IEnumerable<Card> homes
		{
			get
			{
				return null;
			}
		}

		public IEnumerable<Card> frees
		{
			get
			{
				return null;
			}
		}

		public IEnumerable<Card> tableau
		{
			get
			{
				return cards;
			}
		}

		private static readonly IList<Card> map = CardParser.Parse(
			"CA DA HA SA " +
			"C2 D2 H2 S2 " +
			"C3 D3 H3 S3 " +
			"C4 D4 H4 S4 " +
			"C5 D5 H5 S5 " +
			"C6 D6 H6 S6 " +
			"C7 D7 H7 S7 " +
			"C8 D8 H8 S8 " +
			"C9 D9 H9 S9 " +
			"C0 D0 H0 S0 " +
			"CJ DJ HJ SJ " +
			"CQ DQ HQ SQ " +
			"CK DK HK SK"
		).ToArray();

		public static readonly int[] unwinnables = {	// until 1,000,000
			11982, 146692, 186216, 455889, 495505, 512118, 517776, 781948
		};
	}
}