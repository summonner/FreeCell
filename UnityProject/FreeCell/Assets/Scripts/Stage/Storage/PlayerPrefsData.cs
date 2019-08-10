using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {

	public class PlayerPrefsData {
		public int Load( int pageIndex ) {
#if UNITY_EDITOR
			var defaultSaved = StageStates.defaultSaved;
#else
			var defaultSaved = 0;
#endif
			return PlayerPrefs.GetInt( ToKey( pageIndex ), defaultSaved );
		}

		public void Save( int pageIndex, int values ) {
			PlayerPrefs.SetInt( ToKey( pageIndex ), values );
			PlayerPrefs.Save();
		}

		private static string ToKey( int pageIndex ) {
			return pageIndex.ToString();
		}
	}
}