using UnityEngine;
using System.Collections.Generic;

namespace Summoner.Util.PlayerPrefs {
	public interface IPlayerPrefAdaptor<T> {
		T Get( string key, T defaultValue );
		void Set( string key, T value );
	}
}