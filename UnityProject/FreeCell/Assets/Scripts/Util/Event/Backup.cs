using System.Reflection;
using System.Collections.Generic;

namespace Summoner.Util.Event {
	public class Backup {
		private readonly Dictionary<FieldInfo, object> values = new Dictionary<FieldInfo, object>();

		public Backup( System.Type type ) {
			foreach (var evt in type.GetEvents() ) {
				var field = evt.DeclaringType.GetField( evt.Name, BindingFlags.Static | BindingFlags.NonPublic );
				values.Add( field, field.GetValue( null ) );
			}
		}

		public void Apply() {
			foreach ( var field in values ) {
				field.Key.SetValue( null, field.Value );
			}
		}

	}
}