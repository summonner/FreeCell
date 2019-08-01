using UnityEngine;
using System.Collections;

namespace Summoner.Util.StatusBar {
	public interface IStatusBarController : System.IDisposable {
		void Show( bool show );
		int height { get; }
	}
}
