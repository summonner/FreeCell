using UnityEngine;
using System.Collections.Generic;

namespace Summoner {
	public class EnableScope : System.IDisposable {
		private readonly bool oldValue;
		public EnableScope( bool enable ) {
			oldValue = GUI.enabled;
			GUI.enabled = enable;
		}

		public void Dispose() {
			GUI.enabled = oldValue;
		}
	}
}