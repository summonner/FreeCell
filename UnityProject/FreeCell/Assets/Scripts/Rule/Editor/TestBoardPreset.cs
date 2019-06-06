using UnityEngine;

using System.Linq;
using System.Text;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.FreeCell.Test {
	public class TestBoardPreset : IBoardPreset, IBoardLayout {
		private readonly IList<Card> frees;
		private readonly IList<Card> homes;
		private readonly IList<IList<Card>> tableau;
		
		private readonly TestOperation operation = new TestOperation();

		public TestBoardPreset( string freeCells, string homeCells, string[] tableau, bool undo ) {
			var cards = TestCard.Parse( freeCells );
			frees = SelectValue( cards );
			operation.Add( cards, PileId.Type.Free );

			cards = TestCard.Parse( homeCells );
			homes = SelectValue( cards );
			operation.Add( cards, PileId.Type.Home );

			var tables = new List<IList<Card>>();
			for ( int row = 0; row < tableau.Length; ++row ) {
				cards = TestCard.Parse( tableau[row] );
				tables.Add( SelectValue( cards ) );
				operation.Add( cards, row );
			}
			this.tableau = tables.AsReadOnly();

			if ( undo == true ) {
				operation.MakeUndo();
			}
		}

		private IList<Card> SelectValue( IList<TestCard> cards ) {
			return cards.Select( (card) => ( card.value ) )
				.ToList()
				.AsReadOnly();
		}

		private static System.Action Click( PileId.Type type, int column, int row ) {
			var position = new PositionOnBoard( type, column, row );
			return () => {
				PlayerInputEvents.Click( position );
			};
		}

		public void ApplyOperation() {
			operation.Execute();
		}

		public override string ToString() {
			var str = new StringBuilder();
			str.Append( "F[" );
			foreach ( var card in frees ) {
				str.Append( card );
				str.Append( " " );
			}
			str.Append( "][" );
			foreach ( var card in homes ) {
				str.Append( card );
				str.Append( " " );
			}
			str.Append( "]H" );
			str.AppendLine();

			foreach ( var row in tableau ) {
				foreach ( var card in row ) {
					str.Append( card );
					str.Append( " " );
				}
				str.AppendLine();
			}
			return str.ToString();
		}

		public override bool Equals( object obj ) {
			var board = obj as IBoardLookup;
			if ( board == null ) {
				return base.Equals( obj );
			}

			return Equals( board, frees, PileId.Type.Free )
				&& Equals( board, homes, PileId.Type.Home )
				&& Equals( board, tableau );
		}

		public override int GetHashCode() {
			return base.GetHashCode();
		}

		private static bool Equals( IBoardLookup board, IList<Card> cards, PileId.Type type ) {
			for ( int i = 0; i < cards.Count; ++i ) {
				var pile = board[type, i];
				var actual = pile.LastOrDefault();
				if ( cards[i] != actual ) {
					return false;
				}
			}
			return true;
		}

		private static bool Equals( IBoardLookup board, IList<IList<Card>> tableau ) {
			if ( tableau.IsNullOrEmpty() == true ) {
				return board[PileId.Type.Table, 0].IsNullOrEmpty();
			}

			var numPiles = tableau[0].Count;
			for ( int column = 0; column < numPiles; ++column ) {
				var pile = board[PileId.Type.Table, column];
				for ( int row = 0; row < pile.Count; ++row ) {
					if ( SafeGet( tableau, row, column ) != pile[row] ) {
						return false;
					}
				}
			}
			return true;
		}

		private static Card SafeGet( IList<IList<Card>> piles, int row, int column ) {
			if ( piles.IsOutOfRange( row ) == true ) {
				return Card.Blank;
			}

			return piles[row].ElementAtOrDefault( column );
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
				var numColumns = tableau[0].Count;
				foreach ( var row in tableau ) {
					for ( int i=0; i < numColumns; ++i ) {
						if ( row.IsOutOfRange( i ) == true ) {
							yield return Card.Blank;
						}
						else {
							yield return row[i];
						}
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