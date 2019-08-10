using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public class LoadingPopup : MonoBehaviour, System.IDisposable {
		private int count;

		void Awake() {
			UpdateActive();
		}

		private void UpdateActive() {
			gameObject.SetActive( count > 0 );
		}

		public System.IDisposable Show() {
			count += 1;
			UpdateActive();
			return this;
		}

		public void Dispose() {
			Debug.Assert( count > 0 );
			count -= 1;
			UpdateActive();
		}
	}
}