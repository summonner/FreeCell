using UnityEngine.Events;
using System.Collections.Generic;

namespace Summoner {
	public class DisableEvent<T> : System.IDisposable {
		private readonly UnityEvent<T> evt = null;
		private readonly int index = -1;
		public DisableEvent( UnityEvent<T> evt, UnityAction<T> action ) {
			this.evt = evt;
			this.index = evt.IndexOf( action );

			Set( false );
			return;
		}

		public void Dispose() {
			Set( true );
		}

		private void Set( bool enable ) {
			if ( index < 0 ) {
				return;
			}

			var state = enable ? UnityEventCallState.EditorAndRuntime : UnityEventCallState.Off;
			evt.SetPersistentListenerState( index, state );
		}
	}
}