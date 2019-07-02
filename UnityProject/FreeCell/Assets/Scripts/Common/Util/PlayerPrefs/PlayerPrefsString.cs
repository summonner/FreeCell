using UnityEngine;
using System.Collections.Generic;

namespace Summoner.Util.PlayerPrefs {
	public class PlayerPrefsString : IPlayerPrefAdaptor<string> {
		public string Get( string key, string defaultValue ) {
			return UnityEngine.PlayerPrefs.GetString( key, defaultValue );
		}

		public void Set( string key, string value ) {
			UnityEngine.PlayerPrefs.SetString( key, value );
		}
	}
}