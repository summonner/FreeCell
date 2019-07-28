using UnityEngine;
using System.Collections;
using Summoner.Util.Extension;

namespace Summoner.UI.Tween {
	[RequireComponent( typeof(RectTransform) )]
	public class TweenPosition : TweenBase {
		public Vector2 from;
		public Vector2 to;

		private RectTransform _rectTransform;
		private RectTransform rectTransform {
			get {
				return _rectTransform ?? (_rectTransform = GetComponent<RectTransform>());
			}
		}

		void Reset() {
			from = rectTransform.localPosition;
			to = rectTransform.localPosition;
		}

		protected override void LerpValue( float t ) {
			rectTransform.localPosition = Vector2.Lerp( from, to, t );
		}

		[ContextMenu( "Swap" )]
		private void Swap() {
			Swap( ref from, ref to );
		}
	}
}