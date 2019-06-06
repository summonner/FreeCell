using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.FreeCell.Test {
	public static class TestCaseParser {
		private const string undoOperation = "#UNDO";
		private static readonly Regex cells = new Regex( @"\[([^\]]*)\]" );
		public static IEnumerable<TestBoardPreset> Load( string fileName ) {
			var builder = new Builder();

			foreach ( var line in ReadLines( fileName ) ) {
				var matches = cells.Matches( line );
				if ( matches.Count == 2 ) {
					if ( builder.IsEmpty() == false ) {
						yield return builder.Build();
					}
					builder.freeCells = matches[0].Groups[1].Value;
					builder.homeCells = matches[1].Groups[1].Value;
				}
				else if ( line.Equals( undoOperation, System.StringComparison.OrdinalIgnoreCase ) == true ) {
					builder.undo = true;
				}
				else if ( line.IsNullOrEmpty() == false ) {
					builder.tableau.Add( line );
				}
				else if ( builder.IsEmpty() == false ) {
					yield return builder.Build();
				}
			}
		}

		private class Builder {
			public string freeCells = "";
			public string homeCells = "";
			public List<string> tableau = new List<string>();
			public bool undo = false;

			public bool IsEmpty() {
				return freeCells.IsNullOrEmpty() == true
					&& homeCells.IsNullOrEmpty() == true
					&& tableau.IsNullOrEmpty() == true;
			}

			public TestBoardPreset Build() {
				var preset = new TestBoardPreset( freeCells, homeCells, tableau.ToArray(), undo );
				freeCells = "";
				homeCells = "";
				tableau.Clear();
				undo = false;
				return preset;
			}
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
	}
}