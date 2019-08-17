using UnityEngine;
using System.Collections.Generic;

namespace Summoner {
	public static class Lerp {
		public static IEnumerable<float> Duration( float seconds ) {
			var elapsed = 0f;
			while ( elapsed < seconds ) {
				yield return elapsed;
				elapsed += Time.deltaTime;
			}
			yield return seconds;
		}

		public static IEnumerable<float> NormalizedDuration( float seconds ) {
			if ( seconds <= 0f ) {
				yield return 1f;
				yield break;
			}

			foreach ( var elapsed in Duration( seconds ) ) {
				yield return elapsed / seconds;
			}
		}

		public static IEnumerable<float> ElapsedTime() {
			var elapsed = 0f;
			while ( true ) {
				yield return elapsed;
				elapsed += Time.deltaTime;
			}
		}
	}
}