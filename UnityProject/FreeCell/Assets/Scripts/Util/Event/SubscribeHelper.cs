using System;
using System.Reflection;
using System.Collections.Generic;

namespace Summoner.Util.Event {
	public static class SubscribeHelper {

		public static void Subscribe<T>( T listener ) where T : class {
			FindContainerType( listener.GetType(), (containerType, listenerType) => {
				Map( containerType, listenerType, listener, (evt) => ( evt.AddEventHandler ) );
			} );
		}

		public static void Unsubscribe<T>( T listener ) where T : class {
			FindContainerType( listener.GetType(), ( containerType, listenerType ) => {
				Map( containerType, listenerType, listener, (evt) => ( evt.RemoveEventHandler ) );
			} );
		}

		private delegate void OnFind( Type containerType, Type listenerType );
		private static void FindContainerType( Type listenerType, OnFind onFind ) {
			var interfaces = listenerType.GetInterfaces();
			foreach ( var @interface in interfaces ) {
				var attributes = @interface.GetCustomAttributes( typeof( SubscriberOfAttribute ), false );
				foreach ( var attribute in attributes ) {
					var subscriberOf = attribute as SubscriberOfAttribute;
					if ( subscriberOf == null ) {
						continue;
					}

					onFind( subscriberOf.containerType, @interface );
				}
			}
		}

		private delegate void EventHandlerManageFunc( object target, Delegate @delegate );
		private static void Map<T>( Type containerType, Type listenerType, T listener, Func<EventInfo, EventHandlerManageFunc> getManageFunc ) {
			var map = listener.GetType().GetInterfaceMap( listenerType );
			for ( int i = 0; i < map.InterfaceMethods.Length; ++i ) {
				var evt = containerType.GetEvent( map.InterfaceMethods[i].Name );
				var @delegate = Delegate.CreateDelegate( evt.EventHandlerType, listener, map.TargetMethods[i] );
				var manageFunc = getManageFunc( evt );
				manageFunc( null, @delegate );
			}
		}

	}
}