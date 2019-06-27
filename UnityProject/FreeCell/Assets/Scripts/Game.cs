using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Summoner.FreeCell {
	[RequireComponent( typeof(StageSelector) )]
	public class Game : MonoBehaviour {
		[SerializeField] private CardSpriteSheet sheet;
		[SerializeField] private CardObjectHolder cards;
		[SerializeField] private BoardLayout layout;
		[SerializeField] private StageSelector stageSelector;
		[SerializeField] private DemoPlayer demo;
		private IBoardPreset preset;

		private Board board;

		void Start () {
			InGameEvents.OnNoMoreMoves += delegate { Debug.Log( "No More Moves" ); };
			InGameEvents.OnNewGame += NewGame;
			InGameUIEvents.OnReset += OnReset;
			InGameUIEvents.OnCloseTitle += OnCloseTitle;

			board = new Board( layout );
			cards.Init( board, sheet );
			stageSelector.PlayRandomGame();
		}

		void OnDestroy() {
			board.Dispose();
			InGameEvents.OnNewGame -= NewGame;
			InGameUIEvents.OnReset -= OnReset;
			InGameUIEvents.OnCloseTitle -= OnCloseTitle;
		}

		void Reset() {
			sheet = null;
			cards = FindObjectOfType<CardObjectHolder>();
			layout = FindObjectOfType<BoardLayout>();
			stageSelector = GetComponent<StageSelector>();
		}

		private void NewGame( StageNumber stageNumber ) {
			preset = new MSShuffler( stageNumber );
			board.Reset( preset );

			if ( demo != null ) {
				demo.Play( board );
			}
		}

		private void OnReset() {
			board.Reset( preset );
		}

		private void OnCloseTitle() {
			Destroy( demo );
			demo = null;
		}

		private void OnGUI() {
			GUILayout.Space( 50 );
			if ( GUILayout.Button( "Cheat.Clear" ) == true ) {
				InGameEvents.GameClear();
			}
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