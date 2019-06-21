using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Summoner.UI {
	public class RatioToText : BaseToText {
		[SerializeField] private bool addComma = false;
		[SerializeField] private bool allowOverflow = false;

		void Reset() {
			format = "{0} / {1}";
		}

		public void Set( int current, int max ) {
			if ( allowOverflow == false ) {
				current = Mathf.Clamp( current, 0, max );
			}

			var str = ToString( current, max );
			Present( str );
		}

		private IEnumerable<string> ToString( int current, int max ) {
			if ( addComma == true ) {
				yield return NumberToText.AddComma( current );
				yield return NumberToText.AddComma( max );
			}
			else {
				yield return current.ToString();
				yield return max.ToString();
			}
		}
	}
}