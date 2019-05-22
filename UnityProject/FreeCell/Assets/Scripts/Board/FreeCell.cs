using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public class FreeCell : BasePile {
		public FreeCell( int index )
			: base( PileId.Type.Free, index, 1 ) { }
#if UNITY_EDITOR
		public FreeCell()
			: this( 0 ) { }
#endif

		private bool isEmpty {
			get {
				return stack.Count <= 0;
			}
		}

		public override void Push( params Card[] cards ) {
			if ( stack.Count > 0 ) {
				return;
			}

			stack.Add( cards[0] );
		}

		public override Card[] Pop( int index ) {
			if ( isEmpty == true ) {
				return null;
			}

			Debug.Assert( stack.Count == 1 );
			var poped = stack.ToArray();
			stack.Clear();

			return poped;
		}

		public override bool CanMove( int index ) {
			return index < Count;
		}

		public override bool IsAcceptable( params Card[] cards ) {
			return cards.Length == 1
				&& isEmpty == true;
		}
	}
}