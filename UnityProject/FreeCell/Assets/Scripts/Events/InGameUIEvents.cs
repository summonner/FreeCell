using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public class InGameUIEvents : MonoBehaviour {
		public static event System.Action OnUndo = delegate { };
		public void Undo() {
			OnUndo();
		}

		public static event System.Action OnReset = delegate { };
		public void Reset() {
			OnReset();
		}

		private static readonly Util.EventList events = null;
		static InGameUIEvents() {
			events = new Util.EventList( typeof( InGameUIEvents ) );
		}

		public static void Flush() {
			events.Reset();
		}
	}
}