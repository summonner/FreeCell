using UnityEngine;
using System.Collections;
using Summoner.Util.Extension;

namespace Summoner.UI.Tween {
	[RequireComponent( typeof(RectTransform) )]
	public class TweenRotation : TweenBase {
		public float fromDegree;
		public float toDegree;

		private RectTransform _rectTransform;
		private RectTransform rectTransform {
			get {
				return _rectTransform ?? (_rectTransform = GetComponent<RectTransform>());
			}
		}

		void Reset() {
			fromDegree = rectTransform.localRotation.z;
			toDegree = rectTransform.localRotation.z;
		}

		protected override void LerpValue( float t ) {
			var value = Mathf.LerpAngle( fromDegree, toDegree, t );
			rectTransform.localRotation = Quaternion.Euler( 0, 0, value );
		}

		[ContextMenu( "Swap" )]
		private void Swap() {
			Swap( ref fromDegree, ref toDegree );
		}
	}
}