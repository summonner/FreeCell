using UnityEngine;
using System.Collections.Generic;

namespace Summoner.UI.Presenters {
	public class InvertToggleEvent : MonoBehaviour {
		public PresentToggle onEvent = null;

		public void Set( bool value ) {
			onEvent.Invoke( !value );
		}
	}
}