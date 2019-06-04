using UnityEngine;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public class FloatEffect : MonoBehaviour {
		[SerializeField] private new Renderer renderer;
		[SerializeField] private new Collider2D collider;
		[SerializeField] private ContactFilter2D contactFilter;
		private new Transform transform;
		private Vector3 startPosition;
		private Collider2D[] overlapped = new Collider2D[10];

		public void Reset() {
			enabled = false;
			renderer = GetComponentInChildren<Renderer>();
			collider = GetComponent<Collider2D>();
			contactFilter = new ContactFilter2D {
				useLayerMask = true,
				layerMask = LayerMask.GetMask( "Card", "Board" ),
			};
		}

		void Awake() {
			transform = base.transform;
		}

		void OnEnable() {
			renderer.sortingOrder = 1;
			collider.enabled = false;
		}

		void OnDisable() {
			renderer.sortingOrder = 0;
			collider.enabled = true;
		}

		public void Ready() {
			collider.enabled = false;
		}

		public Vector3 Begin() {
			Debug.Assert( transform != null, this );
			enabled = true;
			return startPosition = transform.position;
		}

		public void Move( Vector3 displacement ) {
			Debug.Assert( transform != null, this );
			if ( isActiveAndEnabled == false ) {
				return;
			}

			transform.position = startPosition + displacement;
		}

		public Vector3 End() {
			Debug.Assert( transform != null, this );
			if ( isActiveAndEnabled == false ) {
				return transform.position;
			}

			enabled = false;
			return startPosition;
		}

		public IList<PileId> CheckOverlapped() {
			if ( enabled == false ) {
				return null;
			}

			collider.enabled = true;
			var hits = collider.OverlapCollider( contactFilter, overlapped );
			collider.enabled = false;
			for ( int i = hits; i < overlapped.Length; ++i ) {
				overlapped[i] = null;
			}

			return SelectPileId( overlapped );
		}

		private static IList<PileId> SelectPileId( IList<Collider2D> list ) {
			var result = new List<PileId>( 3 );
			foreach ( var collider in list ) {
				if ( collider == null ) {
					continue;
				}

				var boardObject = collider.GetComponent<IBoardObject>();
				if ( boardObject == null ) {
					continue;
				}

				var id = boardObject.position.pile;
				if ( result.Contains( id ) == true ) {
					continue;
				}

				result.Add( id );
			}

			return result;
		}
	}
}