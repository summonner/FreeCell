using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Summoner.Platform {
	public interface IPlatform : IAuthentication {
		ISavedGame GetSavedGame( string filename );
	}
}