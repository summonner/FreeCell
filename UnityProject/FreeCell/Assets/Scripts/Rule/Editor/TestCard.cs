using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Summoner.Util.Extension;

namespace Summoner.FreeCell.Test {
	public class TestCard {
		public enum Operation {
			None,
			Select,
			Drop,
		}

		private const string operations = @"([\*@]?)";
		private const string blanks = @"[_·]";
		private static readonly string pattern = operations + "(?:" + CardParser.cardPattern + "|" + blanks + ")";
		private static readonly Regex regex = new Regex( pattern );

		public static IList<TestCard> Parse( string cards ) {
			var matches = regex.Matches( cards );
			var results = new TestCard[matches.Count];

			for ( int i = 0; i < matches.Count; ++i ) {
				var token = matches[i].Groups;
				results[i] = new TestCard( token );
			}

			return results.AsReadOnly();
		}

		public readonly Card value = Card.Blank;
		public readonly Operation operation = Operation.None;

		private TestCard( GroupCollection tokens ) {
			operation = ParseOperation( tokens[1].Value );
			value = CardParser.Parse( tokens[2], tokens[3] );
		}

		private static Operation ParseOperation( string token ) {
			switch ( token ) {
				case "*":
					return Operation.Select;
				case "@":
					return Operation.Drop;
				default:
					return Operation.None;
			}
		}
	}
}