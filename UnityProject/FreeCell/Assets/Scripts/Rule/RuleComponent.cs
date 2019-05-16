using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public interface IRuleComponent : System.IDisposable {
		void Reset();
	}
}