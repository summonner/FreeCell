using UnityEngine;
using Summoner.UI;

namespace Summoner.FreeCell {
	public class GameSeed : MonoBehaviour {
		[SerializeField] private int range = 32000;
		[SerializeField] private int testSeed = -1;
		[SerializeField] private PresentInt onChangeSeed = null;

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

			return Random.Range( 1, range );
		}
	}
}