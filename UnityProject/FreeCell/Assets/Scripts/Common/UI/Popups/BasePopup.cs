using UnityEngine;
using System.Collections.Generic;
using Summoner.UI.Popups;

namespace Summoner.UI {
	public class BasePopup : MonoBehaviour, IPopup {
		public PresentToggle onOpenAndClose = null;

		private PopupStack stack {
			get {
				return PopupStack.Instance;
			}
		}


		public void Open() {
			stack.Open( this );
		}

		public void Close() {
			stack.Close( this );
		}

		void IPopup.OnOpen() {
			onOpenAndClose.Invoke( true );

		}

		void IPopup.OnClose() {
			onOpenAndClose.Invoke( false );

		}
	}
}