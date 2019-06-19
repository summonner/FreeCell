using UnityEngine;
using UnityEngine.UI;

namespace Summoner.UI {
	[RequireComponent( typeof( Text ) )]
	public class IntPresenter : BasePresenter {
		[SerializeField] private bool addComma = false;

		public void Set( int value ) {
			Present( ToString( value ) );
		}

		private string ToString( int value ) {
			if ( addComma == true ) {
				return value.ToString( "{0:n0}" );
			}
			else {
				return value.ToString();
			}
		}
	}
}