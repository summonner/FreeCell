using UnityEngine;
using System.Collections;
using Summoner.Util.Extension;

namespace Summoner.UI.Tween {
	[RequireComponent( typeof(RectTransform) )]
	public class TweenScale : TweenBase {
		public Vector2 from;
		public Vector2 to;

		void Reset() {
			from = rectTransform.localScale;
			to = rectTransform.localScale;
		}

		protected override void SetFrame( float t ) {
			rectTransform.localScale = Vector2.Lerp( from, to, t );
		}

		[ContextMenu( "Swap" )]
		private void Swap() {
			Swap( ref from, ref to );
		}
	}
}