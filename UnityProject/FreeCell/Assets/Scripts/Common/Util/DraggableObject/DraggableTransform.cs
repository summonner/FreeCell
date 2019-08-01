using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Summoner.Util.DraggableObject {
	public class DraggableTransform : IDraggableObject {
		private readonly Transform transform;
		private readonly Vector3 startPosition;

		public DraggableTransform( Component component )
			: this( component.transform ) { }

		public DraggableTransform( Transform transform ) {
			this.transform = transform;
			this.startPosition = transform.position;
		}

		public Vector3 OnDrag( PointerEventData eventData ) {
			return OnDrag( eventData, Vector3.one );
		}

		public Vector3 OnDrag( PointerEventData eventData, Vector3 mask ) {
			var displacement = eventData.GetWorldDisplacement( mask );
			OnDrag( displacement );
			return displacement;
		}

		public void OnDrag( Vector3 displacement ) {
			transform.position = startPosition + displacement;
		}
	}
}