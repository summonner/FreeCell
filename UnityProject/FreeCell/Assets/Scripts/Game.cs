﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Summoner.FreeCell {
	public class Game : MonoBehaviour {

		[SerializeField] private CardSpriteSheet sheet;
		[SerializeField] private BoardLayout layout;

		private readonly IList<Card> deck = new List<Card>( Card.NewDeck() ).AsReadOnly();

		private Board board;
		public Vector3 spacing = new Vector3( 1.1f, 0.25f, 0.01f );
		public Vector3 offset = new Vector3( -4f, 5f, 0f );

		void Start () {
			board = new Board( layout );
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
				var row = board.piles[column].Count - 1;
				
				var cardObject = new GameObject( card.ToString() );
				var renderer = cardObject.AddComponent<SpriteRenderer>();
				renderer.sprite = sheet[card];

				var cardTransform = cardObject.transform;
				cardTransform.parent = transform;
				var pos = new Vector3();
				pos.x = (column + offset.x) * spacing.x;
				pos.y = row * spacing.y + offset.y;
				pos.z = row * spacing.z + offset.z;
				cardTransform.position = pos;

				++i;
			}
		}
	}
}