using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public class SoundTrigger : MonoBehaviour {
		[SerializeField] private SoundType type = SoundType.MoveCard;

		public void Play() {
			SoundPlayer.Instance.Play( type );
		}
	}
}