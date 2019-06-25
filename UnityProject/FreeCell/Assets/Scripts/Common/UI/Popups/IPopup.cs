using UnityEngine;
using System.Collections;

namespace Summoner.UI.Popups {
	public interface IPopup {
		void OnOpen();
		void OnClose();
	}
}