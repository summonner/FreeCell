// ref : https://rosettacode.org/wiki/Linear_congruential_generator

namespace Summoner.FreeCell {
	public class MSRandom {
		private const long a = 214013L;
		private const long c = 2531011L;
		private const long m = 1L << 31;
		private const int mod = 1 << 16;

		private int state = 0;

		public MSRandom( int seed ) {
			this.state = seed;
		}

		public int value {
			get {
				MoveNext();
				return state / mod;
			}
		}

		private void MoveNext() {
			state = (int)((a * state + c) % m);
		}

		public int Range( int min, int max ) {
			return min + value % (max - min);
		}
	}
}