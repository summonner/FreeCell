using UnityEngine;
using Summoner.UI;

namespace Summoner.FreeCell {
	public class GameSeed : MonoBehaviour {
		[MinMaxRange( 0f, 100f )]
		[SerializeField] private RangeInt range;
		[SerializeField] private int testSeed = -1;
		[SerializeField] private PresentInt onChangeSeed;

		public int currentSeed { get; private set; }

		public int Generate() {
			currentSeed = GetNewValue();
			onChangeSeed.Invoke( currentSeed );
			return currentSeed;
		}

		private int GetNewValue() {
			if ( testSeed >= 0 ){
				return testSeed;
			}

			return Random.Range( range.min, range.max );
		}
	}
}