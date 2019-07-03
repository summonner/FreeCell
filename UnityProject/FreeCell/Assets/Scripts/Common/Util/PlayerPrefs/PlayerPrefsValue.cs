using UnityEngine;
using System.Collections.Generic;
using Summoner.Util.PlayerPrefs;

namespace Summoner.Util {
	public static class PlayerPrefsValue {
		public static ISavedValue<float> ReadOnlyFloat( string key, float defaultValue ) {
			return new ReadOnlyValue<float>( key, defaultValue, new PlayerPrefsFloat() );
		}

		public static ISavedValue<float> Float( string key, float defaultValue ) {
			return new Value<float>( key, defaultValue, new PlayerPrefsFloat() );
		}

		public static ISavedValue<int> ReadOnlyInt( string key, int defaultValue ) {
			return new ReadOnlyValue<int>( key, defaultValue, new PlayerPrefsInt() );
		}

		public static ISavedValue<int> Int( string key, int defaultValue ) {
			return new Value<int>( key, defaultValue, new PlayerPrefsInt() );
		}

		public static ISavedValue<string> ReadOnlyString( string key, string defaultValue ) {
			return new ReadOnlyValue<string>( key, defaultValue, new PlayerPrefsString() );
		}

		public static ISavedValue<string> String( string key, string defaultValue ) {
			return new Value<string>( key, defaultValue, new PlayerPrefsString() );
		}

		public static ISavedValue<bool> ReadOnlyBool( string key, bool defaultValue ) {
			return new ReadOnlyValue<bool>( key, defaultValue, new PlayerPrefsBool() );
		}

		public static ISavedValue<bool> Bool( string key, bool defaultValue ) {
			return new Value<bool>( key, defaultValue, new PlayerPrefsBool() );
		}


		private class ReadOnlyValue<T> : ISavedValue<T> {
			protected readonly string key;
			private readonly T defaultValue;
			protected readonly IPlayerPrefAdaptor<T> op;

			public ReadOnlyValue( string key, T defaultValue, IPlayerPrefAdaptor<T> op ) {
				this.key = key;
				this.defaultValue = defaultValue;
				this.op = op;
			}

			public virtual T value {
				get {
					return op.Get( key, defaultValue );
				}

				set {
					throw new System.NotSupportedException( "This value is readonly" );
				}
			}

			public override string ToString() {
				return "[" + key + "]" + value.ToString();
			}
		}

		private class Value<T> : ReadOnlyValue<T> {
			public Value( string key, T defaultValue, IPlayerPrefAdaptor<T> op )
				: base( key, defaultValue, op ) { }

			public override T value {
				get {
					return base.value;
				}

				set {
					op.Set( key, value );
					UnityEngine.PlayerPrefs.Save();
				}
			}
		}
	}

}