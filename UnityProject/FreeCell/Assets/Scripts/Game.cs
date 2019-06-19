using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Summoner.FreeCell {
	[RequireComponent( typeof(GameSeed) )]
	public class Game : MonoBehaviour {
		[SerializeField] private CardSpriteSheet sheet;
		[SerializeField] private CardObjectHolder cards;
		[SerializeField] private BoardLayout layout;
		[SerializeField] private GameSeed seed;
		[SerializeField] private DemoPlayer demo;
		private IBoardPreset preset;

		private Board board;

		void Start () {
			InGameEvents.OnNoMoreMoves += delegate { Debug.Log( "No More Moves" ); };
			InGameEvents.OnGameClear += OnClear;
			InGameUIEvents.OnReset += OnReset;
			InGameUIEvents.OnNewGame += OnNewGame;

			board = new Board( layout );
			cards.Init( board, sheet );
			NewGame();
		}

		void OnDestroy() {
			board.Dispose();
			InGameEvents.OnGameClear -= OnClear;
			InGameUIEvents.OnReset -= OnReset;
			InGameUIEvents.OnNewGame -= OnNewGame;
		}

		void Reset() {
			sheet = null;
			cards = FindObjectOfType<CardObjectHolder>();
			layout = FindObjectOfType<BoardLayout>();
			seed = GetComponent<GameSeed>();
		}

		private void NewGame() {
			preset = new MSShuffler( seed.Generate() );
			board.Reset( preset );

			if ( demo != null ) {
				demo.Play( board );
			}
		}

		private void OnClear() {
			Invoke( "NewGame", 1f );
		}

		private void OnReset() {
			board.Reset( preset );
		}

		private void OnNewGame() {
			Destroy( demo );
			demo = null;
			NewGame();
		}

#if UNITY_EDITOR
		[ContextMenu( "Take Board Snapshot" )]
		public void TakeBoardSnapshot() {
			// to check deal number from
			// https://freecellgamesolutions.com/find-game-number
			Debug.Log( board.ToString( "#C" ) );
		}
#endif
	}
}