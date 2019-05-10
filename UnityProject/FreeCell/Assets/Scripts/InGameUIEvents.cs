using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public class InGameUIEvents : MonoBehaviour {
		public delegate void UndoEvent();
		public static event UndoEvent OnUndo = delegate { };
		public void Undo() {
			OnUndo();
		}

		public void Flush() {
			OnUndo = delegate { };
		}
	}
}