using UnityEngine;
using UnityEditor;

namespace Summoner {
	[CustomPropertyDrawer( typeof(ReadonlyAttribute) )]
	public class ReadonlyDrawer : PropertyDrawer {

		public override void OnGUI( Rect position, SerializedProperty property, GUIContent label ) {
			GUI.enabled = false;
			EditorGUI.PropertyField( position, property, label );
			GUI.enabled = true;
		}
	}
}