using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public class GameSeed : MonoBehaviour {
		[MinMaxRange( 0f, 100f )]
		public Range range;
		public int testSeed = -1;
		private int currentSeed = 0;

		public int Get() {
			return currentSeed = GetValue();
		}

		private int GetValue() {
			if ( testSeed >= 0 ){
				return testSeed;
			}

			return Random.Range( (int)range.min, (int)range.max );
		}

		void OnGUI() {
			GUILayout.Label( currentSeed.ToString() );
		}
	}
}