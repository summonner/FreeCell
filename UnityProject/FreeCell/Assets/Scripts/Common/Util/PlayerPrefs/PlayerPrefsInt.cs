using UnityEngine;
using System.Collections.Generic;

namespace Summoner.Util.PlayerPrefs {
	public class PlayerPrefsInt : IPlayerPrefAdaptor<int> {
		public int Get( string key, int defaultValue ) {
			return UnityEngine.PlayerPrefs.GetInt( key, defaultValue );
		}

		public void Set( string key, int value ) {
			UnityEngine.PlayerPrefs.SetInt( key, value );
		}
	}
}