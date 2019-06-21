using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Summoner.UI {
	// ScrollRect를 Scrollbar와 함께 사용할 때 관성에 의한 미끄러짐 부분에 버그가 있음.
	// 스크롤 될 때 Scrollbar의 값을 설정하는데, 특정 구간에서 설정된 값에 의해 스크롤이 멈춤.
	// float에서 (a * b) / b == a 가 보장되지 않는 이슈.

	[RequireComponent( typeof(ScrollRect) )]
	public class ScrollRectFixer : MonoBehaviour {
		[SerializeField] private ScrollRect rect;
		[SerializeField] private Scrollbar horizontal;
		[SerializeField] private Scrollbar vertical;
		[SerializeField] private float threshold = 0.001f;

		void Reset() {
			rect = GetComponent<ScrollRect>();

			horizontal = rect.horizontalScrollbar;
			rect.horizontalScrollbar = null;

			vertical = rect.verticalScrollbar;
			rect.verticalScrollbar = null;
		}

		void Awake() {
			if ( horizontal != null ) {
				horizontal.onValueChanged.AddListener( OnHorizontalValueChanged );
			}
			if ( vertical != null ) {
				vertical.onValueChanged.AddListener( OnVerticalValueChanged );
			}

			rect.onValueChanged.AddListener( OnScroll );
		}

		void OnDestroy() {
			if ( horizontal != null ) {
				horizontal.onValueChanged.RemoveListener( OnHorizontalValueChanged );
			}
			if ( vertical != null ) {
				vertical.onValueChanged.RemoveListener( OnVerticalValueChanged );
			}

			rect.onValueChanged.RemoveListener( OnScroll );
		}

		private void OnHorizontalValueChanged( float value ) {
			if ( IsEquals( rect.horizontalNormalizedPosition, value ) == true ) {
				return;
			}

			rect.horizontalNormalizedPosition = value;
		}

		private void OnVerticalValueChanged( float value ) {
			if ( IsEquals( rect.verticalNormalizedPosition, value ) == true ) {
				return;
			}

			rect.verticalNormalizedPosition = value;
		}

		private bool IsEquals( float left, float right ) {
			return Mathf.Abs( left - right ) < threshold;
		}

		private void OnScroll( Vector2 position ) {
			var size = rect.viewport.rect.size / rect.content.rect.size;

			if ( horizontal != null ) {
				horizontal.value = position.x;
				horizontal.size = size.x;
			}

			if ( vertical != null ) {
				vertical.value = position.y;
				vertical.size = size.y;
			}

		}
	}
}