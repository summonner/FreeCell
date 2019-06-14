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

		public static event System.Action OnNewGame = delegate { };
		public void NewGame() {
			OnNewGame();
		}

		private static readonly Util.Event.Backup initialValues = null;
		static InGameUIEvents() {
			initialValues = new Util.Event.Backup( typeof( InGameUIEvents ) );
		}

		public static void Flush() {
			initialValues.Apply();
		}
	}
}