using UnityEngine;
using System.Collections.Generic;
using Summoner.Util.Event;

namespace Summoner.FreeCell {
	[SubscriberOf( typeof( PlayerInputEvents ) )]
	public interface IDragAndDropListener {
		void OnBeginDrag( int pointerId, PositionOnBoard selected );
		void OnDrag( int pointerId, Vector3 displacement );
		void OnEndDrag( int pointerId );
		void OnDrop( int pointerId, PositionOnBoard selected, IEnumerable<PileId> destination );
	}
}