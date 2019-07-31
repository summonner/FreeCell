using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Summoner {
	public static class PointerEventDataExtension {
		public static Vector2 GetScreenDisplacement( this PointerEventData eventData ) {
			return GetScreenDisplacement( eventData, Vector2.one );
		}

		public static Vector2 GetScreenDisplacement( this PointerEventData eventData, Vector2 mask ) {
			var displacement = eventData.pointerCurrentRaycast.screenPosition - eventData.pointerPressRaycast.screenPosition;
			return Vector2.Scale( displacement, mask );
		}

		public static Vector3 GetWorldDisplacement( this PointerEventData eventData ) {
			return GetWorldDisplacement( eventData, Vector3.one );
		}

		public static Vector3 GetWorldDisplacement( this PointerEventData eventData, Vector3 mask ) {
			var displacement = eventData.pointerCurrentRaycast.worldPosition - eventData.pointerPressRaycast.worldPosition;
			return Vector3.Scale( displacement, mask );
		}
	}
}