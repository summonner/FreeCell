using UnityEngine;
using UnityEditor;

namespace Summoner.EditorExtensions {
	[CustomPropertyDrawer( typeof(MinMaxRangeAttribute) )]
	[CustomPropertyDrawer( typeof(Range) )]
	public class MinMaxRangeDrawer : PropertyDrawer {
		private const float labelLength = 30f;
		private const float margin = 4f;

		public override void OnGUI( Rect position, SerializedProperty property, GUIContent label ) {
			var range = (MinMaxRangeAttribute)attribute;
			if ( range == null ) {
				range = new MinMaxRangeAttribute( 0, 1 );
			}

			var min = property.FindPropertyRelative( range.minProperty );
			var max = property.FindPropertyRelative( range.maxProperty );
			if ( min == null || max == null ) {
				EditorGUI.LabelField( position, property.name + "\nCannot find fields. \"" + range.minProperty + "\", \"" + range.maxProperty + "\"" );
				return;
			}

			var drawMinMaxSlider = GetDrawSliderFunc( min.propertyType );

			var layout = new PropertyLayoutHelper();
			layout.Add( (rect) => { EditorGUI.LabelField( rect, property.name ); } );
			layout.Begin();
			layout.Add( (rect) => EditorGUI.PropertyField( rect, min, GUIContent.none ), labelLength );
			layout.Add( (rect) => drawMinMaxSlider( rect, min, max, range ) );
			layout.Add( (rect) => EditorGUI.PropertyField( rect, max, GUIContent.none ), labelLength );
			layout.End();
			layout.Render( position );
		}

		private delegate void DrawSlider( Rect position, SerializedProperty min, SerializedProperty max, MinMaxRangeAttribute range );
		private DrawSlider GetDrawSliderFunc( SerializedPropertyType type ) {
			switch ( type ) {
				case SerializedPropertyType.Float:
					return MinMaxSlider;

				case SerializedPropertyType.Integer:
					return MinMaxSliderInt;

				default:
					goto case SerializedPropertyType.Float;
			}
		}

		private void MinMaxSlider( Rect position, SerializedProperty min, SerializedProperty max, MinMaxRangeAttribute range ) {
			var minValue = min.floatValue;
			var maxValue = max.floatValue;
			EditorGUI.MinMaxSlider( position, ref minValue, ref maxValue, range.min, range.max );
			min.floatValue = minValue;
			max.floatValue = maxValue;
		}

		private void MinMaxSliderInt( Rect position, SerializedProperty min, SerializedProperty max, MinMaxRangeAttribute range ) {
			var minValue = (float)min.intValue;
			var maxValue = (float)max.intValue;
			EditorGUI.MinMaxSlider( position, ref minValue, ref maxValue, range.min, range.max );
			min.intValue = Mathf.FloorToInt( minValue );
			max.intValue = Mathf.CeilToInt( maxValue );
		}

		public override float GetPropertyHeight( SerializedProperty property, GUIContent label ) {
			return base.GetPropertyHeight( property, label ) * 2;
		}
	}
}