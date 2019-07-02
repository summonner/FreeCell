using UnityEngine;
using System.Collections.Generic;

namespace Summoner.UI {
	[AddComponentMenu( "UI/SVG Image Ex" )]
	public class SVGImageEx : global::SVGImage {

		public override void SetNativeSize() {
			base.SetNativeSize();
			if ( sprite == null ) {
				return;
			}

			var size = sprite.rect.size;
			rectTransform.SetSizeWithCurrentAnchors( RectTransform.Axis.Horizontal, size.x );
			rectTransform.SetSizeWithCurrentAnchors( RectTransform.Axis.Vertical, size.y );
		}
	}
}