using UnityEngine;
using System.Collections.Generic;

namespace Summoner.Util.PlayerPrefs {
	[CreateAssetMenu]
	public class PlayerPrefsEditor : ScriptableObject {
		[SerializeField] private PreferenceValue[] values;
		
		public enum Type {
			Int = 1, Float, String,
		}

		[System.Serializable]
		public struct PreferenceValue {
			public string key;
			public Type type;
		}
	}
}