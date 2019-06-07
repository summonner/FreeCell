using UnityEngine;
using System.Collections;

namespace Summoner.Util.Coroutine {
	public abstract class SubBehaviour {
		private readonly MonoBehaviour outer;

		public SubBehaviour( MonoBehaviour outer ) {
			Debug.Assert( outer != null );
			this.outer = outer;
		}

		protected UnityEngine.Coroutine StartCoroutine( IEnumerator routine ) {
			return outer.StartCoroutine( routine );
		}

		protected void StopCoroutine( UnityEngine.Coroutine routine ) {
			if ( routine == null ) {
				return;
			}

			outer.StopCoroutine( routine );
		}

		protected void StopCoroutine( IEnumerator routine ) {
			if ( routine == null ) {
				return;
			}

			outer.StopCoroutine( routine );
		}
	}
}