using UnityEngine;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public class CardPlacer : MonoBehaviour {
		[SerializeField] private BoardLayout board;

		private Dictionary<Card, Transform> cards = new Dictionary<Card, Transform>( 52 );

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
				var obj = CreateACard( card, sheet );
				cards.Add( card, obj );
			}
		}

		private static Transform CreateACard( Card card, CardSpriteSheet sheet ) {
			var cardObject = new GameObject( card.ToString() );
			var renderer = cardObject.AddComponent<SpriteRenderer>();
			renderer.sprite = sheet[card];

			var transform = cardObject.transform;
			transform.Reset();

			return transform;
		}

		private void OnMoveCards( IEnumerable<Card> targets, PileId destination ) {
			foreach ( var target in targets ) {
				var card = cards[target];
				var pile = board[destination];

				card.parent = pile;
				card.localPosition = new Vector3() {
					y = pile.childCount * board.spacing.y,
					z = pile.childCount * board.spacing.z,
				};
			}
		}
	}
}