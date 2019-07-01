using UnityEngine;
using System.Collections;

namespace Summoner {
	public static class MonoBehaviourExtension {
		public static void DelayedCall( this MonoBehaviour behaviour, YieldInstruction yieldInstruction, System.Action action ) {
			behaviour.StartCoroutine( Delay( yieldInstruction, action ) );
		}

		private static IEnumerator Delay( YieldInstruction yieldInstruction, System.Action action ) {
			yield return yieldInstruction;
			action();
		}
	}
}