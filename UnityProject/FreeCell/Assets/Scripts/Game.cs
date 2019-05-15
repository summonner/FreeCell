using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Summoner.FreeCell {
	public class Game : MonoBehaviour {
		[SerializeField] private CardSpriteSheet sheet;
		[SerializeField] private CardPlacer placer;
		[SerializeField] private BoardLayout layout;
		private BasicPlacement preset;

		private static readonly IList<Card> deck = new List<Card>( Card.NewDeck() ).AsReadOnly();

		private Board board;
		public Vector3 spacing = new Vector3( 1.1f, 0.25f, 0.01f );
		public Vector3 offset = new Vector3( -4f, 5f, 0f );

		void Start () {
			InGameEvents.OnClear += OnClear;
			InGameUIEvents.OnReset += OnReset;
			board = new Board( layout );
			placer.Init( board, sheet, deck );
			NewGame();
		}

		void OnDestroy() {
			board.Dispose();
			InGameEvents.OnClear -= OnClear;
			InGameUIEvents.OnReset -= OnReset;
		}

		void Reset() {
			sheet = null;
			placer = null;
			layout = FindObjectOfType<BoardLayout>();
		}

		private void NewGame() {
			preset = new BasicPlacement( deck, Random.Range( 0, 100 ) );
			board.Reset( preset );
		}

		private void OnClear() {
			Invoke( "NewGame", 1f );
		}

		private void OnReset() {
			board.Reset( preset );
		}
	}
}