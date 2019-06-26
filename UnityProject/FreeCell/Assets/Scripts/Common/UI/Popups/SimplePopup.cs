using UnityEngine;
using System.Collections.Generic;
using Summoner.UI.Popups;

namespace Summoner.UI {
	public class SimplePopup : BasePopup {
		public PresentEvent onOpen = null;
		public PresentEvent onClose = null;

		protected override void OnOpen() {
			onOpen.Invoke();
		}

		protected override void OnClose() {
			onClose.Invoke();
		}
	}
}