using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Summoner.FreeCell {
	[RequireComponent( typeof(GameSeed) )]
	public class Game : MonoBehaviour {
		[SerializeField] private CardSpriteSheet sheet;
		[SerializeField] private CardPlacer placer;
		[SerializeField] private BoardLayout layout;
		[SerializeField] private GameSeed seed;
		private BasicPlacement preset;

		private static readonly IList<Card> deck = new List<Card>( Card.NewDeck() ).AsReadOnly();

		private Board board;

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
			placer = FindObjectOfType<CardPlacer>();
			layout = FindObjectOfType<BoardLayout>();
			seed = GetComponent<GameSeed>();
		}

		private void NewGame() {
			preset = new BasicPlacement( deck, seed.Get() );
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