using UnityEngine;
using System.Collections.Generic;
using Summoner.UI.Popups;

namespace Summoner.FreeCell {
	public class LoadingPopup : MonoBehaviour, IPopup, System.IDisposable {
		private int count;

		void Awake() {
			if ( count <= 0 ) {
				gameObject.SetActive( false );
			}
		}

		public System.IDisposable Show() {
			count += 1;
			PopupStack.Instance.Open( this );
			return this;
		}

		public void Dispose() {
			Debug.Assert( count > 0 );
			count -= 1;
			PopupStack.Instance.Close( this );
		}

		bool IPopup.DoOpen() {
			gameObject.SetActive( count > 0 );
			return count > 0;
		}

		bool IPopup.DoClose() {
			gameObject.SetActive( count > 0 );
			return count <= 0;
		}
	}
}