using UnityEngine;
using System.Collections.Generic;

namespace Summoner.Util {
	public static class Lerp {
		public static IEnumerable<float> Duration( float seconds ) {
			var elapsed = 0f;
			while ( elapsed < seconds ) {
				yield return elapsed;
				elapsed += Time.deltaTime;
			}
			yield return seconds;
		}
	}
}