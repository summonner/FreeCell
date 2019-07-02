using UnityEngine;
using System.Collections.Generic;

namespace Summoner.Util {
	public interface ISavedValue<T> {
		T value { get; set; }
	}
}