using UnityEngine;
using UnityEngine.UI;

namespace Summoner.UI {
	[RequireComponent( typeof( Text ) )]
	public class StringPresenter : BasePresenter {
		public void Set( string value ) {
			Present( value );
		}
	}
}