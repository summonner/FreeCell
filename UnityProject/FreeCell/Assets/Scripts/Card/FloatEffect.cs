using UnityEngine;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public class FloatEffect : MonoBehaviour {
		[SerializeField] private new Renderer renderer;
		[SerializeField] private new Collider2D collider;
		[SerializeField] private GameObject shadow = null;
		[SerializeField] private ContactFilter2D contactFilter;
		private new Transform transform;
		private Collider2D[] overlapped = new Collider2D[10];

		public void Reset() {
			enabled = false;
			renderer = GetComponentInChildren<Renderer>();
			collider = GetComponent<Collider2D>();
			shadow = base.transform.Find( "Shadow" ).gameObject;
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
			shadow.SetActive( true );
		}

		void OnDisable() {
			renderer.sortingOrder = 0;
			collider.enabled = true;
			shadow.SetActive( false );
		}

		public void Begin() {
			Debug.Assert( transform != null, this );
			enabled = true;
		}

		public Vector3 position {
			get {
				return transform.position;
			}
			set {
				transform.position = value;
			}
		}

		public void End() {
			Debug.Assert( transform != null, this );
			if ( isActiveAndEnabled == false ) {
				return;
			}

			enabled = false;
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