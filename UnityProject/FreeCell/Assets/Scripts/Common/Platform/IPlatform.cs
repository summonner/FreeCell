using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Summoner.Platform {
	public interface IPlatform : IAuthentication {
		Task<ISavedGame> FetchSavedGameAsync( string filename );
	}
}