using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.UI {
	public class BaseToText : MonoBehaviour {
		[SerializeField] protected string format = null;
		private ILabel _label = null;
		private ILabel label {
			get {
				return _label ?? (_label = FindLabel( this ));
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

		private static ILabel FindLabel( Component @this ) {
			var text = @this.GetComponent<Text>();
			if ( text != null ) {
				return new TextAdaptor( text );
			}

			var textMesh = @this.GetComponent<TextMeshProUGUI>();
			if ( textMesh != null ) {
				return new TextMeshProUGUIAdaptor( textMesh );
			}

			throw new System.NullReferenceException( "Cannot find any label" );
		}

		private interface ILabel {
			string text { set; }
		}

		private class TextMeshProUGUIAdaptor : ILabel {
			private readonly TextMeshProUGUI textMesh;
			public TextMeshProUGUIAdaptor( TextMeshProUGUI component ) {
				this.textMesh = component;
			}

			public string text {
				set {
					textMesh.text = value;
				}
			}
		}

		private class TextAdaptor : ILabel {
			private readonly Text label;
			public TextAdaptor( Text component ) {
				this.label = component;
			}

			public string text {
				set {
					label.text = value;
				}
			}
		}
	}
}