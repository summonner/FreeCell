using UnityEngine;
using UnityEngine.UI;

namespace Summoner.UI {
	public class NumberToText : BaseToText {
		[SerializeField] private bool addComma = false;

		public void Set( int value ) {
			Present( ToString( value ) );
		}

		private string ToString( int value ) {
			if ( addComma == true ) {
				return AddComma( value );
			}
			else {
				return value.ToString();
			}
		}

		public static string AddComma( int value ) {
			return string.Format( "{0:n0}", value );
		}
	}
}