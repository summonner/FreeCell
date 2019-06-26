using UnityEngine;
using System.Collections;

namespace Summoner.UI.Popups {
	public interface IPopup {
		bool DoOpen();
		bool DoClose();
	}
}