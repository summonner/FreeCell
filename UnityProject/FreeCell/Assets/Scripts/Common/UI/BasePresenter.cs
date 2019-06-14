using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Summoner.UI {
	public class BasePresenter : MonoBehaviour {
		[SerializeField] private Text label;
		[SerializeField] private string format;

		[ContextMenu( "Find Label" )]
		void Reset() {
			label = GetComponent<Text>();
		}

		protected void Present( string value ) {
			Debug.Assert( label != null );
			label.text = string.Format( format, value );
		}
	}
}