using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public class DisableOnStart : MonoBehaviour {

		void Start() {
			gameObject.SetActive ( false );
			Destroy( this );
		}

	}
}