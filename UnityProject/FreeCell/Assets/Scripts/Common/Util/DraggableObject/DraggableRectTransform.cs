using UnityEngine;
using UnityEngine.EventSystems;

namespace Summoner.Util.DraggableObject {
	public class DraggableRectTransform : IDraggableObject {
		private readonly RectTransform transform;
		private readonly Vector2 startPosition;

		public DraggableRectTransform( Component component ) 
			: this( component.GetComponent<RectTransform>() ) { }

		public DraggableRectTransform( RectTransform transform ) {
			this.transform = transform;
			this.startPosition = transform.anchoredPosition;
		}

		public Vector3 OnDrag( PointerEventData eventData ) {
			return OnDrag( eventData, new Vector3( 1f, 1f, 0f ) );
		}

		public Vector3 OnDrag( PointerEventData eventData, Vector3 mask ) {
			var displacement = eventData.GetScreenDisplacement( mask );
			OnDrag( displacement );
			return displacement;
		}

		public void OnDrag( Vector3 displacement ) {
			transform.anchoredPosition = startPosition + (Vector2)displacement;
		}
	}
}