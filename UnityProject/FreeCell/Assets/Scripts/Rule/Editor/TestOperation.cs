using UnityEngine;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.FreeCell.Test {

	public class TestOperation {
		private PositionOnBoard selected = new PositionOnBoard( PileId.Type.Home, -1, -1 );
		private HashSet<PileId> dropTargets = new HashSet<PileId>();
		private bool isUndo = false;

		public void Add( IList<TestCard> cards, PileId.Type type ) {
			foreach ( var card in cards.WithIndex() ) {
				Add( card.Value.operation, type, card.Key, 0 );
			}
		}

		public void Add( IList<TestCard> cards, int row ) {
			foreach ( var card in cards.WithIndex() ) {
				Add( card.Value.operation, PileId.Type.Table, card.Key, row );
			}
		}

		private void Add( TestCard.Operation operation, PileId.Type type, int column, int row ) {
			switch ( operation ) {
				case TestCard.Operation.Select:
					selected = new PositionOnBoard( type, column, row );
					break;
				case TestCard.Operation.Drop:
					var target = new PileId( type, column );
					dropTargets.Add( target );
					break;
				default:
					break;
			}
		}

		public void MakeUndo() {
			isUndo = true;
		}

		public void Execute() {
			if ( isUndo == true ) {
				Undo();
			}
			else if ( dropTargets.IsNullOrEmpty() == false ) {
				DragAndDrop( selected, dropTargets );
			}
			else if ( selected.column >= 0 ) {
				Click( selected );
			}
		}

		private static void Undo() {
			Object.FindObjectOfType<InGameUIEvents>().Undo();
		}

		private static void Click( PositionOnBoard selected ) {
			PlayerInputEvents.Click( selected );
		}

		private static void DragAndDrop( PositionOnBoard selected, IEnumerable<PileId> droppeds ) {
			PlayerInputEvents.SimulateDragAndDrop( selected, droppeds );
		}

		public override string ToString() {
			if ( isUndo == true ) {
				return "Undo";
			}
			else if ( dropTargets.IsNullOrEmpty() == false ) {
				return "DragAndDrop " + selected + " -> " + dropTargets.Join();
			}
			else if ( selected.column >= 0 ) {
				return "Click " + selected;
			}
			return "None";
		}
	}
}