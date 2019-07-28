using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Summoner.Util.Extension;

namespace Summoner.UI.Tween {
	[RequireComponent( typeof(RectTransform) )]
	public class TweenAlpha : TweenBase {
		[Range( 0f, 1f )] public float from;
		[Range( 0f, 1f )] public float to;

		private Graphic _image = null;
		private Graphic image {
			get {
				return _image ?? (_image = GetComponent<Graphic>());
			}
		}

		void Reset() {
			from = image.color.a;
			to = image.color.a;
		}

		protected override void LerpValue( float t ) {
			var color = image.color;
			color.a = Mathf.Lerp( from, to, t );
			image.color = color;
		}

		[ContextMenu( "Swap" )]
		private void Swap() {
			Swap( ref from, ref to );
		}
	}
}