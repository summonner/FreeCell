using UnityEngine;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public class CardPlacer : MonoBehaviour {
		[SerializeField] private BoardLayout layout;
		private IBoardLookup board;

		private Dictionary<Card, CardObject> cards = new Dictionary<Card, CardObject>( 52 );

		void Reset() {
			layout = GetComponent<BoardLayout>();
		}

		void Awake() {
			InGameEvents.OnMoveCards += OnMoveCards;
			InGameEvents.OnCannotMove += OnCannotMove;
		}

		void OnDestroy() {
			InGameEvents.OnMoveCards -= OnMoveCards;
			InGameEvents.OnCannotMove -= OnCannotMove;
		}

		public void Init( IBoardLookup board, CardSpriteSheet sheet, IEnumerable<Card> deck ) {
			this.board = board;

			foreach ( var card in deck ) {
				var obj = sheet.NewObject( card );
				obj.name = card.ToString();
				obj.transform.parent = transform;
				cards.Add( card, obj );
			}
		}

		private void OnMoveCards( IEnumerable<Card> targets, PileId destination ) {
			var pile = board.GetPile( destination );
			var spacing = CalculateSpacing( destination.type );

			foreach ( var target in targets ) {
				var card = cards[target];
				var pilePosition = layout[destination];
				var row = pile.IndexOf( target );
				var position = pilePosition.position + row * spacing;

				card.onClick = () => { InGameEvents.ClickCard( new SelectPosition( destination, row ) ); };
				card.SetPosition( position );
			}
		}

		private void OnCannotMove( IEnumerable<Card> subjects ) {
			foreach ( var subject in subjects ) {
				var card = cards[subject];
				card.Vibrate();
			}
		}

		private Vector3 CalculateSpacing( PileId.Type type ) {
			var spacing = layout.spacing;
			spacing.x = 0f;
			if ( type != PileId.Type.Table ) {
				spacing.y = 0f;
			}
			return spacing;
		}
	}
}