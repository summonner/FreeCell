using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Summoner.UI {
	[System.Serializable]	public class PresentInt : UnityEvent<int> { }
	[System.Serializable]	public class PresentString : UnityEvent<string> { }
}