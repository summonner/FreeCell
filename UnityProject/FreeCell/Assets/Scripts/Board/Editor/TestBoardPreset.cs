using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell.Test {
	public class TestBoardPreset : IBoardPreset {
		public readonly IList<Card> frees;
		public readonly IList<Card> homes;
		public readonly IList<IList<Card>> tableau;

		public readonly SelectPosition select;

		public TestBoardPreset( string freeCells, string homeCells, params string[] tableau ) {
			var cards = new TestCardList( freeCells );
			frees = cards.results;
			if ( cards.targetIndex >= 0 ) {
				select = new SelectPosition( PileId.Type.Free, cards.targetIndex, 0 );
			}

			cards = new TestCardList( homeCells );
			homes = cards.results;
			if ( cards.targetIndex >= 0 ) {
				select = new SelectPosition( PileId.Type.Home, cards.targetIndex, 0 );
			}

			var tables = new List<IList<Card>>();
			for ( int row = 0; row < tableau.Length; ++row ) {
				cards = new TestCardList( tableau[row] );

				tables.Add( cards.results );
				if ( cards.targetIndex >= 0 ) {
					select = new SelectPosition( PileId.Type.Table, cards.targetIndex, row );
				}
			}
			this.tableau = tables.AsReadOnly();
		}

		IEnumerable<Card> IBoardPreset.homes
		{
			get
			{
				return homes;
			}
		}

		IEnumerable<Card> IBoardPreset.frees
		{
			get
			{
				return frees;
			}
		}

		IEnumerable<Card> IBoardPreset.tableau
		{
			get
			{
				foreach ( var row in tableau ) {
					foreach ( var card in row ) {
						yield return card;
					}
				}
			}
		}

		int IBoardLayout.numHomes
		{
			get
			{
				return homes.Count;
			}
		}

		int IBoardLayout.numFrees
		{
			get
			{
				return frees.Count;
			}
		}

		int IBoardLayout.numPiles
		{
			get
			{
				return tableau[0].Count;
			}
		}
	}
}