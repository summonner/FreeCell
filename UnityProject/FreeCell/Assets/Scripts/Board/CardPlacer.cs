using UnityEngine;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public class CardPlacer : MonoBehaviour {
		[SerializeField] private BoardLayout board;

		private Dictionary<Card, CardObject> cards = new Dictionary<Card, CardObject>( 52 );

		void Reset() {
			board = GetComponent<BoardLayout>();
		}

		void Awake() {
			InGameEvents.OnMoveCards += OnMoveCards;
		}

		void OnDestroy() {
			InGameEvents.OnMoveCards -= OnMoveCards;
		}

		public void Init( CardSpriteSheet sheet, IEnumerable<Card> deck ) {
			foreach ( var card in deck ) {
				var obj = sheet.NewObject( card );
				obj.name = card.ToString();
				cards.Add( card, obj );
			}
		}

		private void OnMoveCards( IEnumerable<Card> targets, PileId destination ) {
			foreach ( var target in targets ) {
				var card = cards[target];
				var pile = board[destination];
				var row = pile.childCount;
				var position = new Vector3() {
					y = row * board.spacing.y,
					z = row * board.spacing.z,
				};

				card.onClick = () => { InGameEvents.ClickCard( destination, row ); };
				card.SetPosition( pile, position );
			}
		}
	}
}