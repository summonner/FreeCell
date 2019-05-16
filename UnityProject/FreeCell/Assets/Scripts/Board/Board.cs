using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public interface IBoardLookup {
		IPileLookup this[PileId id] { get; }
		IPileLookup this[PileId.Type type, int index] { get; }
		IEnumerable<IPileLookup> this[params PileId.Type[] types] { get; }
	}

	public interface IBoardController {
		IPile this[PileId pile] { get; }
		IPile this[PileId.Type type, int index] { get; }
		IEnumerable<IPile> this[params PileId.Type[] type] { get; }
	}

	public class Board : IBoardLookup, IBoardController, System.IDisposable {
		private readonly IList<IPile> homes;
		private readonly IList<IPile> frees;
		private readonly IList<IPile> tables;

		private readonly IList<IRuleComponent> ruleComponents;

		public Board( IBoardLayout layout ) {
			homes = Init( layout.numHomes, (i) => ( new HomeCell( i ) ) );
			frees = Init( layout.numFrees, (i) => ( new FreeCell( i ) ) );
			tables = Init( layout.numPiles, (i) => ( new Tableau( i ) ) );
			Debug.Assert( homes != null );
			Debug.Assert( frees != null );
			Debug.Assert( tables != null );

			ruleComponents = new IRuleComponent[] {
				new AutoMove( this ),
				new Undo( this ),
				new ClearCheck( this ),
				new AutoPlayToHome( this ),
			};
		}

#if UNITY_EDITOR
		public Board( IBoardLayout layout, params System.Type[] excludeRules )
			: this( layout )
		{
			foreach ( var rule in ruleComponents ) {
				if ( excludeRules.Contains( rule.GetType() ) == false ) {
					continue;
				}

				rule.Dispose();
			}
		}
#endif

		public void Dispose() {
			foreach ( var component in ruleComponents ) {
				component.Dispose();
			}
		}

		private static IList<IPile> Init<T>( int num, System.Func<int, T> create ) where T : IPile {
			var list = new IPile[num];
			for ( int i=0; i < list.Length; ++i ) {
				list[i] = create( i );
			}
			return list;
		}

		public void Reset( IBoardPreset preset ) {
			ApplyPreset( homes, preset.homes );
			ApplyPreset( frees, preset.frees );
			ApplyPreset( tables, preset.tableau );
			foreach ( var component in ruleComponents ) {
				component.Reset();
			}
		}

		private void ApplyPreset( IList<IPile> target, IEnumerable<Card> preset ) {
			foreach ( var pile in target ) {
				pile.Clear();
			}

			int i = 0;
			foreach ( var card in preset ) {
				if ( card != Card.Blank ) {
					target[i].Push( card );

					var destination = new PileId( PileId.Type.Table, i );
					InGameEvents.SetCard( card, destination );
				}

				i = (i + 1) % target.Count;
			}
		}

		public override string ToString() {
			var str = new System.Text.StringBuilder();
			str.Append( "F[" );
			foreach ( var pile in frees ) {
				str.Append( pile.LastOrDefault() );
				str.Append( " " );
			}
			str.Append( "][" );
			foreach ( var pile in homes ) {
				str.Append( pile.LastOrDefault() );
				str.Append( " " );
			}
			str.Append( "]H" );
			str.AppendLine();

			var maxRow = 0;
			IList<IList<Card>> piles = new IList<Card>[tables.Count];
			for ( int i=0; i < tables.Count; ++i ) {
				piles[i] = tables[i];
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


		private IList<IPile> GetPiles( PileId.Type type ) {
			switch ( type ) {
				case PileId.Type.Free:
					return frees;
				case PileId.Type.Home:
					return homes;
				case PileId.Type.Table:
					return tables;
				default:
					Debug.Assert( false, "Unknown type of pile : " + type );
					return null;
			}
		}

		private IPile Look( PileId id ) {
			var piles = GetPiles( id.type );
			return piles.FirstOrDefault( ( pile ) => (pile.id == id) );
		}

		private IEnumerable<IPile> Traverse( PileId.Type[] types ) {
			foreach ( var type in types ) {
				var piles = GetPiles( type );
				foreach ( var pile in piles ) {
					yield return pile;
				}
			}
		}

		IPileLookup IBoardLookup.this[PileId id] {
			get {
				return Look( id );
			}
		}

		IPileLookup IBoardLookup.this[PileId.Type type, int index] {
			get {
				return Look( new PileId( type, index ) );
			}
		}

		IEnumerable<IPileLookup> IBoardLookup.this[params PileId.Type[] types] {
			get	{
				return Traverse( types ).Cast<IPileLookup>();
			}
		}

		IPile IBoardController.this[PileId id] {
			get	{
				return Look( id );
			}
		}

		IPile IBoardController.this[PileId.Type type, int index] {
			get {
				return Look( new PileId( type, index ) );
			}
		}

		IEnumerable<IPile> IBoardController.this[params PileId.Type[] types] {
			get {
				return Traverse( types );
			}
		}
	}
}