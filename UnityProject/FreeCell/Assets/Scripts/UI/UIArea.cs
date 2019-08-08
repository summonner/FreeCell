using UnityEngine;
using Summoner.Util.DraggableObject;

namespace Summoner.FreeCell {
	[RequireComponent( typeof(RectTransform) )]
	public class UIArea : MonoBehaviour {
		[SerializeField] private Camera uiCamera = null;

		private UISizeAdjustor adjustor = null;
		private IDraggableObject worldCamera = null;
#if !true		// for test
		[Range(0,200)]
		public float height = 0;

		void OnValidate() {
			UpdateHeight();
		}
#else
		private float height = 0;
#endif

		private static Rect safeArea {
			get {
#if UNITY_EDITOR
				var width = 0f;
				var height = 0f;
				return new Rect( 0, 0, Screen.width - width, Screen.height - height );
#else
				return Screen.safeArea;
#endif
			}
		}

		private static void ApplySafeTop( RectTransform transform ) {
			var anchorMax = transform.anchorMax;
			anchorMax.y = safeArea.yMax / Screen.height;
			transform.anchorMax = anchorMax;
		}

		void Start() {
			var transform = GetComponent<RectTransform>();
			adjustor = new UISizeAdjustor( transform, uiCamera );
			worldCamera = new DraggableTransform( Camera.main );
			UpdateHeight();
		}

		public void OnChangeStatusBar( int currentHeight, int previousHeight ) {
			AddTopHeight( currentHeight - previousHeight );
		}

		public void AddTopHeight( int increaments ) {
			this.height += increaments;
			UpdateHeight();
		}

		private void UpdateHeight() {
			if ( adjustor == null ) {
				return;
			}

			var heightDiff = adjustor.Adjust( height );
			var worldDiff = ConvertToWorld( heightDiff );
			worldCamera.OnDrag( -worldDiff );
		}

		private Vector3 ConvertToWorld( float heightDiff ) {
			var zero = Camera.main.ScreenToWorldPoint( Vector3.zero );
			var point = Camera.main.ScreenToWorldPoint( Vector3.down * heightDiff );
			return point - zero;
		}

		private class UISizeAdjustor {
			private readonly RectTransform transform;
			private readonly Camera uiCamera;

			private readonly float screenMargin;
			private readonly float defaultHeight;
			private readonly float sizeAdjustment;
			private const float basisHeight = 1280f;

			private float screenHeight {
				get {
					return uiCamera.pixelHeight;
				}
			}

			public UISizeAdjustor( Component component, Camera uiCamera ) 
				: this ( component.GetComponent<RectTransform>(), uiCamera ) { }

			public UISizeAdjustor( RectTransform transform, Camera uiCamera ) {
				Debug.Assert( transform != null );
				this.transform = transform;
				this.uiCamera = uiCamera;

				this.defaultHeight = transform.rect.height;
				ApplySafeTop( transform );
				this.screenMargin = screenHeight - CalculateTop( transform, uiCamera );
				this.sizeAdjustment = (1 - basisHeight / defaultHeight) * 0.5f * screenHeight;
			}

			private static float CalculateTop( RectTransform transform, Camera uiCamera ) {
				var corners = new Vector3[4];
				transform.GetWorldCorners( corners );
				var leftTop = uiCamera.WorldToScreenPoint( corners[1] );
				return leftTop.y;
			}

			public float Adjust( float height ) {
				var adjustment = Mathf.Max( height, screenMargin );
				transform.SetSizeWithCurrentAnchors( RectTransform.Axis.Vertical, (1 - (adjustment / screenHeight)) * defaultHeight );
				return adjustment - sizeAdjustment;
			}
		}
	}
}