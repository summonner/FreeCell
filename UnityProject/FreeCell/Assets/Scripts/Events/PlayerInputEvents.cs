using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Summoner.FreeCell {

	public static class PlayerInputEvents {
		public delegate void ClickEvent( PositionOnBoard selected );
		public static event ClickEvent OnClick = delegate { };
		public static void Click( PositionOnBoard selected ) {
			OnClick( selected );
		}

		public delegate void BeginDragEvent( int pointerId, PositionOnBoard selected );
		public static event BeginDragEvent OnBeginDrag = delegate { };
		public static void BeginDrag( int pointerId, PositionOnBoard selected ) {
			OnBeginDrag( pointerId, selected );
		}

		public delegate void DraggingEvent( int pointerId, Vector3 displacement );
		public static event DraggingEvent OnDrag = delegate { };
		public static void Drag( int pointerId, Vector3 displacement ) {
			OnDrag( pointerId, displacement );
		}

		public delegate void EndDragEvent( int pointerId );
		public static event EndDragEvent OnEndDrag = delegate { };
		public static void EndDrag( int pointerId ) {
			OnEndDrag( pointerId );
		}

		public delegate void DropEvent( int pointerId, PositionOnBoard selected, IEnumerable<PileId> destination );
		public static event DropEvent OnDrop = delegate { };
		public static void Drop( int pointerId, PositionOnBoard selected, IEnumerable<PileId> destination ) {
			OnDrop( pointerId, selected, destination );
		}

		public static void SimulateDragAndDrop( PositionOnBoard selected, IEnumerable<PileId> destinations ) {
			var temporaryPointerId = 1234;
			BeginDrag( temporaryPointerId, selected );
			Drop( temporaryPointerId, selected, destinations );
			EndDrag( temporaryPointerId );
		}

		public static void Flush() {
			OnClick = delegate { };
			OnBeginDrag = delegate { };
			OnDrag = delegate { };
			OnEndDrag = delegate { };
			OnDrop = delegate { };
		}

		public static void Subscribe( IDragAndDropListener listener ) {
			OnBeginDrag += listener.OnBeginDrag;
			OnDrag += listener.OnDrag;
			OnEndDrag += listener.OnEndDrag;
			OnDrop += listener.OnDrop;
		}

		public static void Unsubscribe( IDragAndDropListener listener ) {
			OnBeginDrag -= listener.OnBeginDrag;
			OnDrag -= listener.OnDrag;
			OnEndDrag -= listener.OnEndDrag;
			OnDrop -= listener.OnDrop;
		}
	}
}