using UnityEngine;
using System.Collections.Generic;

namespace Summoner.UI.Popups {
	public abstract class BasePopup : MonoBehaviour, IPopup {
		protected bool opened { get; private set; }

		public void Open() {
			if ( opened == true ) {
				return;
			}
			PopupStack.Instance.Open( this );
		}

		public void Close() {
			if ( opened == false ) {
				return;
			}
			PopupStack.Instance.Close( this );
		}

		bool IPopup.DoOpen() {
			if ( opened == true ) {
				return false;
			}

			this.OnOpen();
			opened = true;
			return true;
		}

		bool IPopup.DoClose() {
			if ( opened == false ) {
				return false;
			}

			this.OnClose();
			opened = false;
			return true;
		}

		protected abstract void OnOpen();
		protected abstract void OnClose();
	}
}