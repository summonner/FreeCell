using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Summoner.Util.DraggableObject;

namespace Summoner.Util.StatusBar {
	[RequireComponent( typeof(StatusBar) )]
	public class AdjustScreen : MonoBehaviour {
		[SerializeField] private RectTransform uiArea = null;
		[SerializeField] private Camera uiCamera = null;

		private UISizeAdjustor ui;
		private IDraggableObject worldCamera;

		void Start() {
			ui = new UISizeAdjustor( uiArea, uiCamera );
			worldCamera = new DraggableTransform( Camera.main );
		}

#if UNITY_EDITOR
		void Reset() {
			var statusBar = GetComponent<StatusBar>();
			statusBar.onChange.AddListenerIfNotExist( OnStatusBarChange );
		}
#endif

		public void OnStatusBarChange( int height ) {
			var heightDiff = ui.Adjust( height );
			var worldDiff = ConvertToWorld( heightDiff );
			worldCamera.OnDrag( -worldDiff );
		}

		private Vector3 ConvertToWorld( float heightDiff ) {
			var zero = uiCamera.ScreenToWorldPoint( Vector3.zero );
			var point = uiCamera.ScreenToWorldPoint( Vector3.down * heightDiff );
			return point - zero;
		}

		private class UISizeAdjustor {
			private readonly float defaultMargin;
			private readonly float defaultHeight;
			private readonly RectTransform transform;

			public UISizeAdjustor( RectTransform transform, Camera uiCamera ) {
				this.transform = transform;

				this.defaultHeight = transform.rect.height;
				this.defaultMargin = Screen.height - CalculateTop( transform, uiCamera );
			}

			private static float CalculateTop( RectTransform transform, Camera uiCamera ) {
				var corners = new Vector3[4];
				transform.GetWorldCorners( corners );
				var leftTop = uiCamera.WorldToScreenPoint( corners[1] );
				return leftTop.y;
			}

			public float Adjust( float statusBarHeight ) {
				var diff = statusBarHeight - defaultMargin;
				if ( diff < 0 ) {
					diff = 0f;
				}

				transform.SetSizeWithCurrentAnchors( RectTransform.Axis.Vertical, (1 - diff / Screen.height) * defaultHeight );
				return diff;
			}
		}
	}
}