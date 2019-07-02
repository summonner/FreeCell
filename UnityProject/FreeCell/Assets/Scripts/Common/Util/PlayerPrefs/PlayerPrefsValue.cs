using UnityEngine;
using System.Collections.Generic;
using Summoner.Util.PlayerPrefs;

namespace Summoner.Util {
	public static class PlayerPrefsValue {
		public static ISavedValue<float> Float( string key, float defaultValue ) {
			return new Value<float>( key, defaultValue, new PlayerPrefsFloat() );
		}

		public static ISavedValue<int> Int( string key, int defaultValue ) {
			return new Value<int>( key, defaultValue, new PlayerPrefsInt() );
		}

		public static ISavedValue<string> String( string key, string defaultValue ) {
			return new Value<string>( key, defaultValue, new PlayerPrefsString() );
		}

		public static ISavedValue<bool> Bool( string key, bool defaultValue ) {
			return new Value<bool>( key, defaultValue, new PlayerPrefsBool() );
		}


		private class Value<T> : ISavedValue<T> {
			private readonly string key;
			private readonly T defaultValue;
			private readonly IPlayerPrefAdaptor<T> op;

			public Value( string key, T defaultValue, IPlayerPrefAdaptor<T> op ) {
				this.key = key;
				this.defaultValue = defaultValue;
				this.op = op;
			}

			public T value {
				get {
					return op.Get( key, defaultValue );
				}

				set {
					op.Set( key, value );
					UnityEngine.PlayerPrefs.Save();
				}
			}
		}
	}

}