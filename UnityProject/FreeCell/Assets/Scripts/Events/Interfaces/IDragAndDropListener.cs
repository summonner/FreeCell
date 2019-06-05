using UnityEngine;
using System.Collections.Generic;
using Summoner.Util.Event;

namespace Summoner.FreeCell {
	[SubscriberOf( typeof( PlayerInputEvents ) )]
	public interface IDragAndDropListener {
		void OnBeginDrag( PositionOnBoard selected );
		void OnDrag( PositionOnBoard selected, Vector3 displacement );
		void OnEndDrag( PositionOnBoard selected );
		void OnDrop( PositionOnBoard selected, IEnumerable<PileId> destination );
	}
}