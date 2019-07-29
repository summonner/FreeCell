using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public class CardObjectHolder : MonoBehaviour {
		private static readonly IList<Card> deck = Card.NewDeck().ToList().AsReadOnly();

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

		public void Init( IBoardLookup board, CardSpriteSheet sheet ) {
			this.board = board;

			foreach ( var card in deck ) {
				var obj = sheet.NewObject( card );
				obj.name = card.ToString();
				obj.transform.parent = transform;
				cards.Add( card, obj );
			}
		}
		
		private static readonly Vector3 offset = Vector3.back * 0.01f; 
		public System.Action MoveCard( Card target, PileId to, float volume ) {
			var card = cards[target];
			var row = board[to].IndexOf( target );
			var boardPosition = new PositionOnBoard( to, row );
			var worldPosition = layout.GetWorldPosition( boardPosition ) + offset;

			return card.SetDestination( boardPosition, worldPosition, volume );
		}

		public IEnumerable<System.Action> MoveCard( IEnumerable<Card> targets, PileId to ) {
			foreach ( var target in targets.WithIndex() ) {
				var volume = target.Key == 0 ? 1f : 0f;
				yield return MoveCard( target.Value, to, volume );
			}
		}

		private IEnumerable<CardObject> Find( IEnumerable<Card> subjects ) {
			foreach ( var subject in subjects ) {
				yield return cards[subject];
			}
		}

		private void OnCannotMove( IEnumerable<Card> subjects ) {
			SoundPlayer.Instance.Play( SoundType.CannotMove );
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
			foreach ( var card in Find( subjects ).WithIndex() ) {
				var volume = card.Key == 0 ? 1f : 0f;
				card.Value.EndFloat( volume );
			}
		}

		public IEnumerable<System.Action> OnReset() {
			var position = transform.position;
			foreach ( var card in cards.Values ) {
				yield return card.SetDestination( default( PositionOnBoard ), position, 0f );
			}
			yield return () => { SoundPlayer.Instance.Play( SoundType.ResetCards ); };
		}

	}
}