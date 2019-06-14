using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public class DemoPlayer : MonoBehaviour {
		[SerializeField] private InGameUIEvents ui;
		private bool isCleared;
		
		void OnEnable() {
			InGameEvents.OnGameClear += OnClear;
		}

		void OnDisable() {
			InGameEvents.OnGameClear -= OnClear;
		}

		private IBoardLookup board;
		public void Play( IBoardLookup board ) {
			this.board = board;
			StartCoroutine( PlayAux( board ) );
		}

		private static readonly YieldInstruction wait = new WaitForSeconds( 1f );
		private IDictionary<string, IList<MoveForDemo>> map = new Dictionary<string, IList<MoveForDemo>>();
		private MoveForDemo last;
		private IEnumerator PlayAux( IBoardLookup board ) {
			var hasher = new BoardHasher( board );
			var finder = new PossibleMoveFinder( board );
			isCleared = false;
			map.Clear();

			yield return new WaitForSeconds( 2f );
			while ( isCleared == false ) {
				var hash = hasher.Generate();
				if ( map.ContainsKey( hash ) == false ) {
					var possibleMoves = (from move in finder.FindMoves()
										where move.cards.FirstOrDefault() != last.@from.card
										select new MoveForDemo( board, move ))
									   .ToList();
					map.Add( hash, possibleMoves );
				}

				var moves = map[hash];
				yield return wait;

				if ( moves.IsNullOrEmpty() == true ) {
					ui.Undo();
				}
				else {
					last = ExtractAMove( moves );
					ApplyMove( last );
				}

			}
		}

		private T ExtractAMove<T>( IList<T> moves ) {
			var selected = Random.Range( 0, moves.Count );
			var move = moves[selected];
			moves.RemoveAt( selected );
			return move;
		}

		private void ApplyMove( MoveForDemo move ) {
			var from = Find( move.from );
			var to = Find( move.to );
			PlayerInputEvents.SimulateDragAndDrop( from, new[] { to.pile } );
		}

		private PositionOnBoard Find( CardToMove move ) {
			var piles = board[move.type];
			foreach ( var pile in piles ) {
				if ( move.card == Card.Blank ) {
					if ( pile.Count == 0 ) {
						return new PositionOnBoard( pile.id, 0 );
					}
				}
				else {
					var index = pile.IndexOf( move.card );
					if ( index >= 0 ) {
						return new PositionOnBoard( pile.id, index );
					}
				}
			}

			Debug.Assert( false, "Cannot find card : " + move );
			return new PositionOnBoard( PileId.Type.Home, -1, -1 );
		}

		private void OnClear() {
			isCleared = true;
		}

		private struct MoveForDemo {
			public readonly CardToMove from;
			public readonly CardToMove to;

			public MoveForDemo( IBoardLookup board, Move move ) {
				from = new CardToMove( move.cards.FirstOrDefault(), move.from.type );
				to = new CardToMove( board[move.to].LastOrDefault(), move.to.type );
			}
		}

		private struct CardToMove {
			public readonly Card card;
			public readonly PileId.Type type;

			public CardToMove( Card card, PileId.Type type ) {
				this.card = card;
				this.type = type;
			}

			public override string ToString() {
				return type + " " + card;
			}
		}
	}
}