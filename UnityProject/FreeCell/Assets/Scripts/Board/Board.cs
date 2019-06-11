using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public class Board : IBoardLookup, IBoardController, System.IDisposable {
		private readonly IDictionary<PileId, IPile> piles = new Dictionary<PileId, IPile>();
		private readonly IList<IRuleComponent> ruleComponents;
		private readonly IBoardLayout layout;

		public Board( IBoardLayout layout ) {
			this.layout = layout;
			var homes = Create( layout.GetNumber( PileId.Type.Home ), (i) => ( new HomeCell( i ) ) );
			var frees = Create( layout.GetNumber( PileId.Type.Free ), (i) => ( new FreeCell( i ) ) );
			var tables = Create( layout.GetNumber( PileId.Type.Table ), (i) => ( new Tableau( i ) ) );
			Debug.Assert( homes != null );
			Debug.Assert( frees != null );
			Debug.Assert( tables != null );

			foreach ( var pile in homes.Concat( frees ).Concat( tables ) ) {
				piles.Add( pile.id, pile );
			}
			Debug.Assert( piles.Count == homes.Count + frees.Count + tables.Count );

			ruleComponents = new IRuleComponent[] {
				new AutoMove( this ),
				new Undo( this ),
				new ClearCheck( this ),
				new AutoPlayToHome( this ),
				new DragAndDrop( this ),
			//	new PossibleMoveFinder( this ),	// integrated to AutoPlayToHome
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

		private static IList<IPile> Create<T>( int num, System.Func<int, T> create ) where T : IPile {
			var list = new IPile[num];
			for ( int i=0; i < list.Length; ++i ) {
				list[i] = create( i );
			}
			return list;
		}

		public void Reset( IBoardPreset preset ) {
			InGameEvents.ClearBoard();
			ApplyPreset( PileId.Type.Home, preset.homes );
			ApplyPreset( PileId.Type.Free, preset.frees );
			ApplyPreset( PileId.Type.Table, preset.tableau );
			foreach ( var component in ruleComponents ) {
				component.Reset();
			}
		}

		private void ApplyPreset( PileId.Type type, IEnumerable<Card> preset ) {
			var target = Traverse( type ).ToList();
			foreach ( var pile in target ) {
				pile.Clear();
			}

			int i = 0;
			foreach ( var card in preset ) {
				if ( card != Card.Blank ) {
					target[i].Push( card );
					InGameEvents.InitBoard( card, target[i].id );
				}

				i = (i + 1) % target.Count;
			}
		}

		public override string ToString() {
			var str = new System.Text.StringBuilder();
			str.Append( "F[" );
			foreach ( var pile in Traverse( PileId.Type.Free ) ) {
				str.Append( pile.LastOrDefault() );
				str.Append( " " );
			}
			str.Append( "][" );
			foreach ( var pile in Traverse( PileId.Type.Home ) ) {
				str.Append( pile.LastOrDefault() );
				str.Append( " " );
			}
			str.Append( "]H" );
			str.AppendLine();

			var tables = Traverse( PileId.Type.Table ).ToList();
			var maxRow = tables.Max( (pile) => ( pile.Count ) );
			for ( int row = 0; row < maxRow; ++row ) {
				for ( int column = 0; column < tables.Count; ++column ) {
					var current = tables[column];
					var card = current.IsOutOfRange( row ) ? Card.Blank : current[row];
					str.Append( card );
					str.Append( " " );
				}
				str.AppendLine();
			}
			return str.ToString();
		}


		private IPile Look( PileId id ) {
			return piles[id];
		}

		private IEnumerable<IPile> Traverse( params PileId.Type[] types ) {
			foreach ( var type in types ) {
				var numPiles = layout.GetNumber( type );
				for ( int i=0; i < numPiles; ++i ) {
					var id = new PileId( type, i );
					yield return Look( id );
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

		IBoardLookup IBoardController.AsReadOnly() {
			return this;
		}
	}
}