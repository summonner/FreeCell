using UnityEngine;

namespace Summoner.Util.Extension {
	public static class TransformExtension {

		public static void Reset( this Transform transform ) {
			transform.localPosition = Vector3.zero;
			transform.rotation = Quaternion.identity;
			transform.localScale = Vector3.one;
		}

		public static void Reset( this Transform transform, Transform parent ) {
			transform.parent = parent;
			transform.Reset();
		}
	}
}