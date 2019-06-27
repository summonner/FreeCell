using UnityEngine;
using System.Collections.Generic;
using Summoner.Util.Singleton;

namespace Summoner.FreeCell {
	public class InGameUIEvents : SingletonBehaviour<InGameUIEvents> {
		public static event System.Action OnUndo = delegate { };
		public void Undo() {
			OnUndo();
		}

		public static event System.Action OnReset = delegate { };
		public void Reset() {
			OnReset();
		}

		public static event System.Action OnCloseTitle = delegate { };
		public void CloseTitle() {
			OnCloseTitle();
		}

		public static event System.Action OnQuickGame = delegate { };
		public void QuickGame() {
			OnQuickGame();
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