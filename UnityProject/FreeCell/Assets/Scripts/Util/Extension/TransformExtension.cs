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

		public static T GetComponent<T>( this Transform transform ) { 
			return transform.gameObject.GetComponent<T>();
		}

		public static T GetOrAddComponent<T>( this Transform transform ) where T : Component {
			return transform.gameObject.GetOrAddComponent<T>();
		}

		public static T GetOrAddComponent<T>( this GameObject gameObject ) where T : Component {
			var component = gameObject.GetComponent<T>();
			if ( component != null ) {
				return component;
			}

			return gameObject.AddComponent<T>();
		}
	}
}