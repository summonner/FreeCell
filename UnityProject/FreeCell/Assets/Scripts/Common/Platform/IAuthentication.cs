using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Summoner.Platform {
	public interface IAuthentication {
		Task<bool> AuthenticateAsync( bool silent );
		bool isAuthenticated { get; }
		void SignOut();
	}
}