using UnityEngine;
using System.Collections;

namespace Summoner.Util {
	public abstract class SubBehaviour {
		private readonly MonoBehaviour outer;

		public SubBehaviour( MonoBehaviour outer ) {
			this.outer = outer;
		}

		protected Coroutine StartCoroutine( IEnumerator routine ) {
			return outer.StartCoroutine( routine );
		}

		protected void StopCoroutine( Coroutine routine ) {
			outer.StopCoroutine( routine );
		}

		protected void StopCoroutine( IEnumerator routine ) {
			outer.StopCoroutine( routine );
		}
	}
}