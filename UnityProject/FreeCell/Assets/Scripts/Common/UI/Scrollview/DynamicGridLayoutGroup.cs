using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;
using System.Collections.Generic;

namespace Summoner.UI {
	[System.Serializable]	public class InitGridItem : UnityEvent<int, GameObject> { }

	public class DynamicGridLayoutGroup : LayoutGroup, ILayoutSelfController {
		public enum Axis { Horizontal = 0, Vertical = 1 }

		[SerializeField] private ScrollRect view = null;
		[SerializeField] private Vector2 cellSize = new Vector2( 100f, 100f );
		[SerializeField] private Vector2 spacing = Vector2.zero;
		[SerializeField] private Axis direction = Axis.Vertical;
		[SerializeField] private int constraintCount = 0;
		[SerializeField] private GameObject gridItem = null;
		[SerializeField] private int _numItems = 1;
		public InitGridItem onInitGridItem = null;

		private Properties properties;
		private IDictionary<int, RectTransform> visibles = new Dictionary<int, RectTransform>();
		private ObjectPool<Transform> pool;

		protected override void Reset() {
			base.Reset();
			view = GetComponentInParent<ScrollRect>();
		}

		protected override void Awake() {
			base.Awake();
			pool = new ObjectPool<Transform>( () => ( Instantiate( gridItem, transform ).transform ) );

			AdjustChildrenCount();
			if ( view != null ) {
				view.onValueChanged.AddListener( OnScroll );
			}
		}

		public int numItems {
			get {
				return _numItems;
			}
			set {
				_numItems = value;
				AdjustChildrenCount();
			}
		}

		[ContextMenu("Rebuild Children")]
		private void AdjustChildrenCount() {
			visibles.Clear();
			properties = new Properties( this );

			RemoveChildren( transform.childCount );
			GenerateChildren( properties.numChildren );
		}

		private void RemoveChildren( int numToRemove ) {
#if UNITY_EDITOR
			if ( Application.isPlaying == false ) {
				for ( int i=0; i < numToRemove; ++i ) {
					DestroyImmediate( transform.GetChild( 0 ).gameObject );
				}
				return;
			}
#endif
			foreach ( Transform child in transform ) {
				if ( child.gameObject.activeInHierarchy == false ) {
					continue;
				}

				if ( numToRemove <= 0 ) {
					break;
				}

				pool.Push( child );
				numToRemove -= 1;
			}
		}

		private void GenerateChildren( int numToGenerate ) {
			for ( int i=0; i < numToGenerate; ++i ) {
				pool.Pop();
			}
		}

		public override void CalculateLayoutInputHorizontal() {
			RebuildRectChildren();	// instead base.CalculateLayoutInputHorizontal();
			this.properties = new Properties( this );

			var size = properties.totalSize.x;
			SetLayoutInputForAxis( size, size, -1, 0 );
		}

		private void RebuildRectChildren() {
			rectChildren.Clear();
			for ( int i = 0; i < rectTransform.childCount; i++ ) {
				RectTransform rect = rectTransform.GetChild( i ) as RectTransform;
				if ( rect == null )
					continue;
				ILayoutIgnorer ignorer = rect.GetComponent( typeof( ILayoutIgnorer ) ) as ILayoutIgnorer;
				if ( rect.gameObject.activeInHierarchy && !(ignorer != null && ignorer.ignoreLayout) )
					rectChildren.Add( rect );
			}
		}

		public override void CalculateLayoutInputVertical() {
			var size = properties.totalSize.y;
			SetLayoutInputForAxis( size, size, -1, 1 );
		}

		public override void SetLayoutHorizontal() {
			m_Tracker.Clear();
			foreach ( var child in rectChildren ) {
				m_Tracker.Add( this, child, 
					DrivenTransformProperties.Anchors |
					DrivenTransformProperties.AnchoredPosition |
					DrivenTransformProperties.SizeDelta );

				child.anchorMin = Vector2.up;
				child.anchorMax = Vector2.up;
				child.sizeDelta = cellSize;
			}

			m_Tracker.Add( this, rectTransform, DrivenTransformProperties.SizeDelta );
			var totalSize = properties.totalSize;
			rectTransform.SetSizeWithCurrentAnchors( RectTransform.Axis.Horizontal, totalSize.x );
			rectTransform.SetSizeWithCurrentAnchors( RectTransform.Axis.Vertical, totalSize.y );
		}

		public override void SetLayoutVertical() {
			var viewRange = CalculateViewRange();
			UpdateVisibles( viewRange );

			foreach ( var child in visibles ) {
				SetChildTo( child.Key, child.Value );
			}
		}

		private RangeInt CalculateViewRange() {
			var startIndex = properties.GetStartIndex( view.normalizedPosition );
			startIndex = Mathf.Min( startIndex, numItems - rectChildren.Count );
			return new RangeInt( startIndex, startIndex + rectChildren.Count );
		}

		private void UpdateVisibles( RangeInt viewRange ) {
			var invisibles = FindInvisibles( viewRange ).GetEnumerator();
			var shows = viewRange.GetEnumerable().Except( visibles.Keys );

			foreach ( var index in shows ) {
				if ( invisibles.MoveNext() == false ) {
					break;
				}

				var invisible = invisibles.Current;
				visibles.Add( index, invisible );
				onInitGridItem.Invoke( index, invisible.gameObject );
			}
		}

		private IEnumerable<RectTransform> FindInvisibles( RangeInt viewRange ) {
			var hides = visibles.Keys.Except( viewRange.GetEnumerable() ).ToArray();
			foreach ( var i in hides ) {
				visibles.Remove( i );
			}

			return rectChildren.Except( visibles.Values ).ToArray();
		}

		private void SetChildTo( int index, RectTransform child ) {
#if UNITY_EDITOR
			child.name = index.ToString();
#endif
			var position = properties.IndexToPosition( index );
			SetChildAlongAxis( child, 0, position.x, cellSize.x );
			SetChildAlongAxis( child, 1, position.y, cellSize.y );
		}

		public void OnScroll( Vector2 viewPosition ) {
			SetLayoutVertical();
		}

		private class Properties {
			private static readonly Vector2 extra = Vector2.one * 0.001f;

			private readonly Vector2 startOffset;
			private readonly Vector2 length;
			private readonly Vector2 itemSize;
			public readonly Vector2 totalSize;
			public int numChildren { 
				get {
					return directionalProperties.minItems;
				}
			}

			private readonly IDirectionalProperties directionalProperties;

			public Properties( DynamicGridLayoutGroup outer ) {
				if ( outer.view == null ) {
					directionalProperties = new PropertiesForHorizontal( Vector2.zero, 0, 0 );
					return;
				}

				var padding = new Vector2( outer.padding.horizontal, outer.padding.vertical );
				var margin = padding - outer.spacing;
				this.itemSize = outer.cellSize + outer.spacing;

				var viewSize = outer.view.viewport.GetComponent<RectTransform>().rect.size;
				var visibleCount = (viewSize - margin + extra) / itemSize;

				if ( outer.direction == Axis.Vertical ) {
					directionalProperties = new PropertiesForVertical( visibleCount, outer.numItems, outer.constraintCount );
				}
				else {
					directionalProperties = new PropertiesForHorizontal( visibleCount, outer.numItems, outer.constraintCount );
				}

				var count = new Vector2( directionalProperties.numColumns, directionalProperties.numRows );
				var requiredSpace = totalSize - margin;
				this.startOffset = new Vector2( outer.GetStartOffset( 0, requiredSpace.x ), outer.GetStartOffset( 1, requiredSpace.y ) );
				this.totalSize = margin + count * itemSize;
				this.length = totalSize - viewSize;
			}

			public Vector2 IndexToPosition( int i ) {
				var position = directionalProperties.IndexToPosition( i );
				return startOffset + position * itemSize;
			}

			public int GetStartIndex( Vector2 position ) {
				position.y = 1 - position.y;
				var start = length * position;
				var p = start / itemSize;
				var startIndex = directionalProperties.GetStartIndex( p );
				return Mathf.Max( 0, startIndex );
			}

			private interface IDirectionalProperties {
				int numColumns { get; }
				int numRows { get; }
				int minItems { get; }
				Vector2 IndexToPosition( int i );
				int GetStartIndex( Vector2 position );
			}

			private class PropertiesForHorizontal : IDirectionalProperties {
				public int numColumns { get; private set; }
				public int numRows { get; private set; }
				public int minItems { get; private set; }

				public Vector2 IndexToPosition( int i ) {
					return new Vector2( i / numRows, i % numRows );
				}

				public PropertiesForHorizontal( Vector2 visibleCount, int numItems, int constraint ) {
					numRows = (constraint > 0) ? constraint : Mathf.FloorToInt( visibleCount.y );
					numColumns = Mathf.CeilToInt( (float)numItems / numRows );
					minItems = numRows * (Mathf.CeilToInt( visibleCount.x ) + 1);
					minItems = Mathf.Min( minItems, numItems );
				}

				public int GetStartIndex( Vector2 position ) {
					var startColumn = Mathf.FloorToInt( position.x );
					return numRows * startColumn;
				}
			}

			private class PropertiesForVertical : IDirectionalProperties {
				public int numColumns { get; private set; }
				public int numRows { get; private set; }
				public int minItems { get; private set; }

				public Vector2 IndexToPosition( int i ) {
					return new Vector2( i % numColumns, i / numColumns );
				}

				public PropertiesForVertical( Vector2 visibleCount, int numItems, int constraint ) {
					numColumns = (constraint > 0) ? constraint : Mathf.FloorToInt( visibleCount.x );
					numRows = Mathf.CeilToInt( (float)numItems / numColumns );
					minItems = numColumns * (Mathf.CeilToInt( visibleCount.y ) + 1);
					minItems = Mathf.Min( minItems, numItems );
				}

				public int GetStartIndex( Vector2 position ) {
					var startRow = Mathf.FloorToInt( position.y );
					return numColumns * startRow;
				}
			}
		}
	}
}