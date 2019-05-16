using UnityEngine;
using UnityEditor;

namespace Summoner.EditorExtension {
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

			var minProperty = property.FindPropertyRelative( "min" );
			var maxProperty = property.FindPropertyRelative( "max" );
			if ( minProperty == null || maxProperty == null ) {
				EditorGUI.LabelField( position, property.name + "\nCannot find fields. \"min\", \"max\"" );
				return;
			}

			var min = minProperty.floatValue;
			var max = maxProperty.floatValue;

			var layout = new PropertyLayoutHelper();
			layout.Add( (rect) => { EditorGUI.LabelField( position, property.name ); } );
			layout.Begin();
			layout.Add( (rect) => { min = EditorGUI.FloatField( rect, min ); }, labelLength );
			layout.Add( (rect) => EditorGUI.MinMaxSlider( rect, ref min, ref max, range.min, range.max ) );
			layout.Add( (rect) => { max = EditorGUI.FloatField( rect, max ); }, labelLength );
			layout.End();
			layout.Render( position );

			minProperty.floatValue = min;
			maxProperty.floatValue = max;
		}

		public override float GetPropertyHeight( SerializedProperty property, GUIContent label ) {
			return base.GetPropertyHeight( property, label ) * 2;
		}
	}
}