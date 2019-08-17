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
		public UnityEvent BackButtonFallback;

		public void Open( IPopup target ) {
			Debug.Assert( target != null );
			if ( target.DoOpen() == false ) {
				return;
			}
			stack.Push( target );
		}

		public void Close( IPopup target ) {
			Debug.Assert( target != null );
			if ( stack.Peek() != target ) {
				Debug.LogError( "Invalid order of close popup.\nCurrent stack : " + stack.Join() );
			}

			CloseLastPopup();
		}

		void Update() {
			if ( Input.GetKeyDown( KeyCode.Escape ) == true ) {
				CloseLastPopup();
			}
		}

		public void CloseLastPopup() {
			if ( stack.Count <= 0 ) {
				BackButtonFallback.Invoke();
				return;
			}

			var last = stack.Peek();
			if ( last.DoClose() == false ) {
				 return;
			}

			stack.Pop();
		}

		public void OpenPopup( GameObject gameObject ) {
			SendEvent( gameObject, Open );
		}

		public void ClosePopup( GameObject gameObject ) {
			SendEvent( gameObject, Close );
		}

		private delegate void PopupEvent( IPopup popup );
		private static void SendEvent( GameObject gameObject, PopupEvent eventFunc ) {
			var popup = gameObject.GetComponent<IPopup>();
			if ( popup == null ) {
				Debug.LogError( gameObject.name + " is not a popup object", gameObject );
				return;
			}

			eventFunc( popup );
		}

#if UNITY_EDITOR
		public Stack<IPopup> GetStack() {
			return stack;
		}
#endif
	}
}