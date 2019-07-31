using UnityEngine;
using UnityEngine.EventSystems;

namespace Summoner.Util {
	public interface IDraggableObject {
		Vector3 OnDrag( PointerEventData eventData );
		Vector3 OnDrag( PointerEventData eventData, Vector3 mask );
		void OnDrag( Vector3 displacement );
	}
}