﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Summoner.FreeCell {
	[RequireComponent( typeof(StageManager) )]
	public class Game : MonoBehaviour {
		[SerializeField] private CardSpriteSheet sheet;
		[SerializeField] private CardObjectHolder cards;
		[SerializeField] private BoardLayout layout;
		[SerializeField] private InGameUIEvents uiEvents;
		[SerializeField] private DemoPlayer demo;
		private IBoardPreset preset;

		private Board board;

		void Start() {
			InGameEvents.OnNewGame += NewGame;
			InGameUIEvents.OnReset += OnReset;
			InGameUIEvents.OnCloseTitle += OnCloseTitle;

			board = new Board( layout );
			cards.Init( board, sheet );

			uiEvents.QuickGame();
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
			uiEvents = FindObjectOfType<InGameUIEvents>();
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

#if UNITY_EDITOR
		private void OnGUI() {
			using ( new GUILayout.HorizontalScope() ) {
				GUILayout.Space( 200 );
				if ( GUILayout.Button( "Cheat.Clear" ) == true ) {
					var dummy = new PileId( PileId.Type.Table, 0 );
					foreach ( var card in Card.NewDeck() ) {
						InGameEvents.AutoPlay( new [] { card }, dummy, new PileId( PileId.Type.Home, (int)card.suit - 1 ) );
					}
					InGameEvents.GameClear();
				}
				if ( GUILayout.Button( "Slow" ) == true ) {
					Time.timeScale = Time.timeScale == 1f ? 0.1f : 1f;
				}
			}
		}

		[ContextMenu( "Take Board Snapshot" )]
		public void TakeBoardSnapshot() {
			// to check deal number from
			// https://freecellgamesolutions.com/find-game-number
			Debug.Log( board.ToString( "#C" ) );
		}
#endif
	}
}