using UnityEngine;
using System.Reflection;
using System.Collections.Generic;

namespace Summoner.Util {
	public class EventList {
		private readonly Dictionary<FieldInfo, object> initialValues = new Dictionary<FieldInfo, object>();

		public EventList( System.Type type ) {
			foreach (var evt in type.GetEvents() ) {
				var field = evt.DeclaringType.GetField( evt.Name, BindingFlags.Static | BindingFlags.NonPublic );
				initialValues.Add(field, field.GetValue( null ) );
			}
		}

		public void Reset() {
			foreach ( var field in initialValues ) {
				field.Key.SetValue( null, field.Value );
			}
		}
	}
}