using UnityEngine;

namespace Summoner.FreeCell {
	[RequireComponent( typeof(RectTransform) )]
	public class BondUI : MonoBehaviour {
		[SerializeField] private RectTransform follower = null;
		private PivotedPosition my;
		private PivotedPosition other;
		private System.Action onLateUpdate = delegate { };

		public void Init() {
			my = new PivotedPosition( new TopPosition( this ) );
			other = new PivotedPosition( new WorldPosition( follower ) );
			onLateUpdate = () => { other.displacement = my.displacement; };
		}

		void OnDisable() {
			onLateUpdate();
		}

		void LateUpdate() {
			onLateUpdate();
		}

		private class PivotedPosition {
			private readonly IPosition transform;
			private readonly Vector3 pivot;

			public PivotedPosition( IPosition transform ) {
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

		private interface IPosition {
			Vector3 position { get; set; }
		}

		private class TopPosition : IPosition {
			private readonly RectTransform transform;
			public TopPosition( Component component ) {
				this.transform = component.GetComponent<RectTransform>();
			}

			public Vector3 position {
				get {
					return CalculateTop( transform );
				}
				set {
					throw new System.NotImplementedException();
				}
			}

			private static Vector3 CalculateTop( RectTransform transform ) {
				var corners = new Vector3[4];
				transform.GetWorldCorners( corners );
				var top = corners[1].y;
				return Vector3.up * top;
			}
		}

		private class WorldPosition : IPosition {
			private readonly RectTransform transform;
			public WorldPosition( RectTransform transform ) {
				this.transform = transform;
			}

			public Vector3 position {
				get {
					return transform.position;
				}
				set {
					transform.position = value;
				}
			}
		}
	}
}