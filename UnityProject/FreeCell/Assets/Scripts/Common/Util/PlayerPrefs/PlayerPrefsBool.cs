using UnityEngine;
using System.Collections.Generic;

namespace Summoner.Util.PlayerPrefs {
	public class PlayerPrefsBool : IPlayerPrefAdaptor<bool> {
		private static int Convert( bool value ) {
			return value ? 1 : 0;
		}

		private static bool Convert( int value ) {
			return value != 0;
		}

		public bool Get( string key, bool defaultValue ) {
			var value = UnityEngine.PlayerPrefs.GetInt( key, Convert( defaultValue ) );
			return Convert( value );
		}

		public void Set( string key, bool value ) {
			UnityEngine.PlayerPrefs.SetInt( key, Convert( value ) );
		}
	}
}