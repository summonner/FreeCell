using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Summoner.FreeCell {
	public class Game : MonoBehaviour {

		[SerializeField] private CardSpriteSheet sheet;
		[SerializeField] private CardPlacer placer;
		[SerializeField] private BoardLayout layout;

		private readonly IList<Card> deck = new List<Card>( Card.NewDeck() ).AsReadOnly();

		private Board board;
		public Vector3 spacing = new Vector3( 1.1f, 0.25f, 0.01f );
		public Vector3 offset = new Vector3( -4f, 5f, 0f );

		void Start () {
			board = new Board( layout );
			placer.Init( sheet, deck );
			Initialize();
		}

		void Reset() {
			layout = FindObjectOfType<BoardLayout>();
		}

		private void Initialize() {
			board.Clear();

			var i = 0;
			var cards = Util.Random.FisherYatesShuffle.Draw( deck );
			foreach ( var card in cards ) {
				var column = i % board.piles.Count;
				board.piles[column].Push( card );

				var destination = new PileId( PileId.Type.Table, column );
				InGameEvents.MoveACard( card, destination );
				++i;
			}
		}
	}
}