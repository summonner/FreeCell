using UnityEngine;
using System.Collections;
using Summoner.Util.Extension;

namespace Summoner.UI.Tween {
	[RequireComponent( typeof(RectTransform) )]
	public class TweenPosition : TweenBase {
		public Vector2 from;
		public Vector2 to;

		void Reset() {
			from = rectTransform.localPosition;
			to = rectTransform.localPosition;
		}

		protected override void SetFrame( float t ) {
			rectTransform.localPosition = Vector2.Lerp( from, to, t );
		}

		[ContextMenu( "Swap" )]
		private void Swap() {
			Swap( ref from, ref to );
		}
	}
}