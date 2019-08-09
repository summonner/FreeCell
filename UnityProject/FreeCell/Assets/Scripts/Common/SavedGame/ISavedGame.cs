using UnityEngine;
using System.IO;
using System.Collections.Generic;

namespace Summoner.SavedGame {
	public interface ISavedGame {
		byte[] data { get; set; }
	}
}