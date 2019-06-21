using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.UI {
	[RequireComponent( typeof( Text ) )]
	public class BaseToText : MonoBehaviour {
		[SerializeField] protected string format = null;
		private Text _label = null;
		private Text label {
			get {
				return _label ?? (_label = GetComponent<Text>());
			}
		}

		protected void Present( IEnumerable<string> values ) {
			Present( values.ToArray() );
		}

		protected void Present( params string[] values ) {
			Debug.Assert( label != null );
			label.text = ApplyFormat( values );
		}

		private string ApplyFormat( string[] values ) {
			if ( format.IsNullOrEmpty() == true ) {
				return values[0];
			}

			return string.Format( format, values );
		}
	}
}