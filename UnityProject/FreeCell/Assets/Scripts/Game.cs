using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Summoner.FreeCell {
	public class Game : MonoBehaviour {
		[SerializeField] private CardSpriteSheet sheet;
		[SerializeField] private CardPlacer placer;
		[SerializeField] private BoardLayout layout;
		private System.Random random = new System.Random( 0 );

		private readonly IList<Card> deck = new List<Card>( Card.NewDeck() ).AsReadOnly();

		private Board board;
		public Vector3 spacing = new Vector3( 1.1f, 0.25f, 0.01f );
		public Vector3 offset = new Vector3( -4f, 5f, 0f );

		void Start () {
			InGameEvents.OnClear += OnClear;
			board = new Board( layout );
			placer.Init( board, sheet, deck );
			Initialize();
		}

		void OnDestroy() {
			board.Dispose();
			InGameEvents.OnClear -= OnClear;
		}

		void Reset() {
			sheet = null;
			placer = null;
			layout = FindObjectOfType<BoardLayout>();
		}

		private void Initialize() {
			var cards = Util.Random.FisherYatesShuffle.Draw( deck, random.Next );
			board.Reset( cards );
		}

		private void OnClear() {
			Debug.Log( "Clear!!" );
		}
	}
}