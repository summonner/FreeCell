using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.UI {
	public class BasePresenter : MonoBehaviour {
		[SerializeField] private Text label = null;
		[SerializeField] private string format = null;

		[ContextMenu( "Find Label" )]
		void Reset() {
			label = GetComponent<Text>();
		}

		protected void Present( string value ) {
			Debug.Assert( label != null );
			label.text = ApplyFormat( value );
		}

		private string ApplyFormat( string value ) {
			if ( format.IsNullOrEmpty() == true ) {
				return value;
			}

			return string.Format( format, value );
		}
	}
}