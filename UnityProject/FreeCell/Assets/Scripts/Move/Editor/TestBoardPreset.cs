using UnityEngine;

using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Summoner.Util.Extension;

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

		private static readonly Regex cells = new Regex( @"\[([^\]]*)\]" );
		public static IEnumerable<TestBoardPreset> Load( string fileName ) {
			var freeCells = "";
			var homeCells = "";
			var tableau = new List<string>();

			foreach ( var line in ReadLines( fileName ) ) {
				var matches = cells.Matches( line );
				if ( matches.Count == 2 ) {
					freeCells = matches[0].Groups[1].Value;
					homeCells = matches[1].Groups[1].Value;
				}
				else if ( line.IsNullOrEmpty() == false ) {
					tableau.Add( line );
				}
				else if ( IsEmpty( freeCells, homeCells, tableau ) == false ) {
					yield return new TestBoardPreset( freeCells, homeCells, tableau.ToArray() );
					freeCells = "";
					homeCells = "";
					tableau.Clear();
				}
			}
		}

		private static bool IsEmpty( string freeCells, string homeCells, IList<string> tableau ) {
			return freeCells.IsNullOrEmpty() == true
				&& homeCells.IsNullOrEmpty() == true
				&& tableau.IsNullOrEmpty() == true;
		}

		private const string testCasePath = "/TestCases/";
		private const string extension = ".txt";
		private static IEnumerable<string> ReadLines( string fileName ) {
			var file = File.ReadAllLines( Application.dataPath + testCasePath + fileName + extension );
			foreach ( var line in file ) {
				yield return line.Trim();
			}

			yield return "";
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