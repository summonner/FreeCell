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

		public delegate void BeginDragEvent( PositionOnBoard selected );
		public static event BeginDragEvent OnBeginDrag = delegate { };
		public static void BeginDrag( PositionOnBoard selected ) {
			OnBeginDrag( selected );
		}

		public delegate void DraggingEvent( PositionOnBoard selected, Vector3 displacement );
		public static event DraggingEvent OnDrag = delegate { };
		public static void Drag( PositionOnBoard selected, Vector3 displacement ) {
			OnDrag( selected, displacement );
		}

		public static event BeginDragEvent OnEndDrag = delegate { };
		public static void EndDrag( PositionOnBoard selected ) {
			OnEndDrag( selected );
		}

		public delegate void DropEvent( PositionOnBoard selected, IEnumerable<PileId> destination );
		public static event DropEvent OnDrop = delegate { };
		public static void Drop( PositionOnBoard selected, IEnumerable<PileId> destination ) {
			OnDrop( selected, destination );
		}

		public static void SimulateDragAndDrop( PositionOnBoard selected, IEnumerable<PileId> destinations ) {
			BeginDrag( selected );
			Drop( selected, destinations );
			EndDrag( selected );
		}

		private static readonly System.Type type = typeof( PlayerInputEvents );
		private static readonly Util.Event.Backup initialValues = null;
		static PlayerInputEvents() {
			initialValues = new Util.Event.Backup( type );
		}

		public static void Flush() {
			initialValues.Apply();
		}
	}
}