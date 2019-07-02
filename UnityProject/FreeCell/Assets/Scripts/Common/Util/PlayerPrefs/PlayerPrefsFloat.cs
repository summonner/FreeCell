using UnityEngine;
using System.Collections.Generic;

namespace Summoner.Util.PlayerPrefs {
	public class PlayerPrefsFloat : IPlayerPrefAdaptor<float> {
		public float Get( string key, float defaultValue ) {
			return UnityEngine.PlayerPrefs.GetFloat( key, defaultValue );
		}

		public void Set( string key, float value ) {
			UnityEngine.PlayerPrefs.SetFloat( key, value );
		}
	}
}