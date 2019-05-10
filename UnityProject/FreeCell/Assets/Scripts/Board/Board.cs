﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public interface IBoardLookup {
		IList<Card> Look( PileId id );
	}

	public interface IBoardController {
		IList<IPile> this[PileId.Type type] { get; }
		IPile this[PileId pile] { get; }
	}

	public class Board : IBoardLookup, IBoardController, System.IDisposable {
		private readonly IList<IPile> homes;
		private readonly IList<IPile> frees;
		private readonly IList<IPile> tables;

		private readonly IList<System.IDisposable> ruleComponents = new List<System.IDisposable>();

		public Board( IBoardLayout layout ) {
			homes = Init<HomeCell>( layout.numHomes );
			frees = Init<FreeCell>( layout.numFrees );
			tables = Init<Tableau>( layout.numPiles );
			Debug.Assert( homes != null );
			Debug.Assert( frees != null );
			Debug.Assert( tables != null );

			ruleComponents.Add( new MoveRule( this ) );
			ruleComponents.Add( new CommandStack( this ) );
		}

		public Board( IBoardPreset preset ) 
			: this( (IBoardLayout)preset ) 
		{
			Clear();
			ApplyPreset( homes, preset.homes );
			ApplyPreset( frees, preset.frees );
			ApplyPreset( tables, preset.tableau );
		}

		public void Dispose() {
			foreach ( var component in ruleComponents ) {
				component.Dispose();
			}
		}

		private void ApplyPreset( IList<IPile> target, IEnumerable<Card> preset ) {
			int i=0;
			foreach ( var card in preset ) {
				if ( card != Card.Blank ) {
					target[i].Push( card );
				}

				i = (i + 1) % target.Count;
			}
		}

		private static IList<IPile> Init<T>( int num ) where T : IPile, new() {
			var list = new IPile[num];
			for ( int i=0; i < list.Length; ++i ) {
				list[i] = new T();
			}
			return list;
		}

		public void Clear() {
			Clear( homes );
			Clear( frees );
			Clear( tables );
		}

		private static void Clear( IList<IPile> piles ) {
			foreach ( var pile in piles ) {
				pile.Clear();
			}
		}

		public void Reset( IEnumerable<Card> cards ) {
			Clear();

			var i = 0;
			foreach ( var card in cards ) {
				var column = i % tables.Count;
				tables[column].Push( card );

				var destination = new PileId( PileId.Type.Table, column );
				InGameEvents.SetCard( card, destination );
				++i;
			}
		}

		public override string ToString() {
			var str = new System.Text.StringBuilder();
			str.Append( "F[" );
			foreach ( var pile in frees ) {
				var cards = pile.GetReadOnly();
				str.Append( cards.LastOrDefault() );
				str.Append( " " );
			}
			str.Append( "][" );
			foreach ( var pile in homes ) {
				var cards = pile.GetReadOnly();
				str.Append( cards.LastOrDefault() );
				str.Append( " " );
			}
			str.Append( "]H" );
			str.AppendLine();

			var maxRow = 0;
			IList<IList<Card>> piles = new IList<Card>[tables.Count];
			for ( int i=0; i < tables.Count; ++i ) {
				piles[i] = tables[i].GetReadOnly();
				maxRow = Mathf.Max( maxRow, piles[i].Count );
			}
			for ( int row = 0; row < maxRow; ++row ) {
				for ( int column = 0; column < piles.Count; ++column ) {
					var current = piles[column];
					var card = current.IsOutOfRange( row ) ? Card.Blank : current[row];
					str.Append( card );
					str.Append( " " );
				}
				str.AppendLine();
			}
			return str.ToString();
		}

		IList<Card> IBoardLookup.Look( PileId id ) {
			var piles = GetPiles( id.type );
			if ( piles.IsOutOfRange( id.index ) == true ) {
				return null;
			}

			return piles[id.index].GetReadOnly();
		}

		private IList<IPile> GetPiles( PileId.Type type ) {
			switch ( type ) {
				case PileId.Type.Free:
					return frees;
				case PileId.Type.Home:
					return homes;
				case PileId.Type.Table:
					return tables;
				default:
					return null;
			}
		}

		IList<IPile> IBoardController.this[PileId.Type type] {
			get {
				return GetPiles( type );
			}
		}

		IPile IBoardController.this[PileId pile] {
			get {
				var piles = GetPiles( pile.type );
				return piles.ElementAtOrDefault( pile.index );
			}
		}
	}
}