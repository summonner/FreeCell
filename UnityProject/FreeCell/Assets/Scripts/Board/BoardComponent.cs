using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public class BoardComponent : MonoBehaviour, IBoardObject {
		public PositionOnBoard position { get; set; }
	}
}