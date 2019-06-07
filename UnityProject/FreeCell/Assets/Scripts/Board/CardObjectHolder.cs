using UnityEngine;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public class CardObjectHolder : MonoBehaviour {
		[SerializeField] private BoardLayout layout;
		private IBoardLookup board;
		private Dictionary<Card, CardObject> cards = new Dictionary<Card, CardObject>( 52 );

		void Reset() {
			layout = GetComponent<BoardLayout>();
		}

		void Awake() {
			InGameEvents.OnCannotMove += OnCannotMove;
			InGameEvents.OnBeginFloatCards += OnBeginFloatCards;
			InGameEvents.OnFloatCards += OnFloatCards;
			InGameEvents.OnEndFloatCards += OnEndFloatCards;
		}

		void OnDestroy() {
			InGameEvents.OnCannotMove -= OnCannotMove;
			InGameEvents.OnBeginFloatCards -= OnBeginFloatCards;
			InGameEvents.OnFloatCards -= OnFloatCards;
			InGameEvents.OnEndFloatCards -= OnEndFloatCards;
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
		
		private static readonly Vector3 offset = Vector3.back * 0.01f; 
		public System.Action MoveCard( Card target, PileId to ) {
			var card = cards[target];
			var row = board[to].IndexOf( target );
			card.position = new PositionOnBoard( to, row );
			var position = layout.GetWorldPosition( card.position ) + offset;

			return card.SetDestination( position );
		}

		private IEnumerable<CardObject> Find( IEnumerable<Card> subjects ) {
			foreach ( var subject in subjects ) {
				yield return cards[subject];
			}
		}

		private void OnCannotMove( IEnumerable<Card> subjects ) {
			foreach ( var card in Find( subjects ) ) {
				card.Vibrate();
			}
		}

		private void OnBeginFloatCards( IEnumerable<Card> subjects ) {
			foreach ( var card in Find( subjects ) ) {
				card.BeginFloat();
			}
		}

		private void OnFloatCards( IEnumerable<Card> subjects, Vector3 displacement ) {
			foreach ( var card in Find( subjects ) ) {
				card.Float( displacement );
			}
		}

		private void OnEndFloatCards( IEnumerable<Card> subjects ) {
			foreach ( var card in Find( subjects ) ) {
				card.EndFloat();
			}
		}

		public void OnReset() {
			var position = transform.position;
			foreach ( var card in cards.Values ) {
				var trigger = card.SetDestination( position );
				trigger();
			}
		}
	}
}