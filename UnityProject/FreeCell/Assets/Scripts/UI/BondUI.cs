using UnityEngine;

namespace Summoner.FreeCell {
	[RequireComponent( typeof(RectTransform) )]
	public class BondUI : MonoBehaviour {
		[SerializeField] private RectTransform follower = null;
		private PivotedPosition my;
		private PivotedPosition other;
		private System.Action onLateUpdate = delegate { };

		public void Init() {
			my = new PivotedPosition( this );
			other = new PivotedPosition( follower );
			onLateUpdate = () => { other.displacement = my.displacement;	};
		}

		void OnDisable() {
			other.displacement = Vector3.zero;
		}

		void LateUpdate() {
			onLateUpdate();
		}

		private class PivotedPosition {
			private readonly RectTransform transform;
			private readonly Vector3 pivot;

			public PivotedPosition( Component component ) 
				: this( component.GetComponent<RectTransform>() ) { }

			public PivotedPosition( RectTransform transform ) {
				this.transform = transform;
				pivot = transform.position;
			}
			
			public Vector3 displacement {
				get {
					return transform.position - pivot;
				}
				set {
					transform.position = pivot + value;
				}
			}
		}
	}
}