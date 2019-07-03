using UnityEngine;
using UnityEditor;

using System.Linq;
using System.Collections.Generic;

namespace Summoner.EditorExtensions {
	public class PropertyLayoutHelper {
		public static Rect AdjustRect( Rect rect, float height ) {
			return AdjustRect( rect, height, 0.5f );
		}

		public static Rect AdjustRect( Rect rect, float height, float pivot ) {
			var margin = rect.height - height;
			rect.yMin += margin * pivot;
			rect.yMax -= margin * (1 - pivot);
			return rect;
		}

		private Stack<SubRectList> stack = new Stack<SubRectList>();

		public PropertyLayoutHelper() {
			stack.Push( new SubRectList( true ) );
		}

		public delegate void RenderFunc( Rect rect );
		public void Add( RenderFunc render ) {
			Add( render, 0 );
		}

		public void Add( RenderFunc render, float length ) {
			stack.Peek().Add( new SubRect( render, length ) );
		}

		public void Begin() {
			var current = stack.Peek();
			var list = new SubRectList( !current.isVertical );
			current.Add( list );
			stack.Push( list );
		}

		public void End() {
			stack.Pop();
		}

		public void Render( Rect position ) {
			var indent = EditorGUI.indentLevel;
			position = EditorGUI.IndentedRect( position );

			EditorGUI.indentLevel = 0;
			stack.Peek().Render( position );
			EditorGUI.indentLevel = indent;
		}

		private interface ISubRect {
			float length { get; }
			void Add( ISubRect sub );
			void Render( Rect position );
		}

		private class SubRect : ISubRect {
			public float length { get; private set; }
			private readonly RenderFunc render;

			public SubRect( RenderFunc render, float length ) {
				this.length = length;
				this.render = render;
			}

			public void Add( ISubRect sub ) {
				throw new System.NotSupportedException();
			}

			public void Render( Rect position ) {
				if ( render == null ) {
					return;
				}

				render( position );
			}
		}

		private class SubRectList : ISubRect {
			private List<ISubRect> subs = new List<ISubRect>();
			public readonly bool isVertical = true;
			private static readonly float spacing = EditorGUIUtility.standardVerticalSpacing;

			public SubRectList( bool isVertical ) {
				this.isVertical = isVertical;
			}

			public float length {
				get {
					return 0;
				}
			}

			public void Add( ISubRect sub ) {
				subs.Add( sub );
			}

			public void Render( Rect position ) {
				if ( isVertical == true ) {
					RenderY( position );
				}
				else {
					RenderX( position );
				}
			}

			public void RenderY( Rect position ) {
				var lengths = Divide( position.height );
				var start = position.y;
				for ( int i = 0; i < lengths.Count; ++i ) {
					var rect = new Rect( position.x, start, position.width, lengths[i] );
					subs[i].Render( rect );
					start += lengths[i] + spacing;
				}
			}

			public void RenderX( Rect position ) {
				var lengths = Divide( position.width );
				var start = position.x;
				for ( int i = 0; i < lengths.Count; ++i ) {
					var rect = new Rect( start, position.y, lengths[i], position.height );
					subs[i].Render( rect );
					start += lengths[i] + spacing;
				}
			}

			private IList<float> Divide( float length ) {
				var numAuto = subs.Count( ( item ) => (item.length <= 0) );
				var sum = subs.Sum( ( item ) => (Mathf.Max( 0, item.length )) );
				length = length - spacing * (subs.Count - 1);
				var autoLength = (length - sum) / numAuto;

				var result = new float[subs.Count];
				for ( int i = 0; i < subs.Count; ++i ) {
					var current = subs[i].length;
					if ( current > 0 ) {
						result[i] = current;
					}
					else {
						result[i] = autoLength;
					}
				}

				return result;
			}
		}
	}
}