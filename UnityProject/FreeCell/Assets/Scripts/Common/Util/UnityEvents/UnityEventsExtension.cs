#if UNITY_EDITOR
using UnityEditor.Events;
#endif
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Summoner {
	public static class UnityEventsExtension {
		public static int IndexOf<T>( this UnityEvent<T> evt, UnityAction<T> action ) {
			var count = evt.GetPersistentEventCount();
			for ( int i = 0; i < count; ++i ) {
				var isTarget = evt.GetPersistentTarget( i ) == (action.Target as Object)
							&& evt.GetPersistentMethodName( i ) == action.Method.Name;
				if ( isTarget == false ) {
					continue;
				}

				return i;
			}

			return -1;
		}

#if UNITY_EDITOR
		public static void AddListenerIfNotExist<T>( this UnityEvent<T> evt, UnityAction<T> action ) {
			if ( evt.IndexOf( action ) >= 0 ) {
				return;
			}

			UnityEventTools.AddPersistentListener( evt, action );
		}
#endif
	}
}