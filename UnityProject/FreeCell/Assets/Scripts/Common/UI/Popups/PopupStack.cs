using UnityEngine;
using System.Collections.Generic;
using Summoner.Util.Singleton;
using Summoner.Util.Extension;

namespace Summoner.UI.Popups {
	public class PopupStack : SingletonBehaviour<PopupStack> {
		public static PopupStack Instance {
			get {
				return instance;
			}
		}

		private readonly Stack<IPopup> stack = new Stack<IPopup>( 2 );

		public void Open( BasePopup target ) {
			Open( (IPopup)target );
		}

		public void Open( IPopup target ) {
			target.OnOpen();
			stack.Push( target );
		}

		public void Close( BasePopup target ) {
			Close( (IPopup)target );
		}

		public void Close( IPopup target ) {
			target.OnClose();
			if ( stack.Peek() != target ) {
				Debug.LogError( "Invalid order of close popup.\nCurrent stack : " + stack.Join() );
			}
			stack.Pop();
		}

		void Update() {
			if ( Input.GetKeyDown( KeyCode.Escape ) == true ) {
				OnBackButtonPressed();
			}
		}

		private void OnBackButtonPressed() {
			if ( stack.Count <= 0 ) {
				return;
			}

			var last = stack.Pop();
			last.OnClose();
		}
	}
}