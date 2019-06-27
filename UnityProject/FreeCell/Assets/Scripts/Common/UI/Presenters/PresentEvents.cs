using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Summoner.UI {
	[System.Serializable] public class PresentInt : UnityEvent<int> { }
	[System.Serializable] public class PresentString : UnityEvent<string> { }
	[System.Serializable] public class PresentRatio : UnityEvent<int, int> { }
	[System.Serializable] public class PresentToggle : UnityEvent<bool> { }
	[System.Serializable] public class UnityEvent : UnityEngine.Events.UnityEvent { }
}