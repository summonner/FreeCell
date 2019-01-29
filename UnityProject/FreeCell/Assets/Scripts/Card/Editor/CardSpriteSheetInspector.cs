using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Summoner.FreeCell {
	[CustomEditor( typeof( CardSpriteSheet ) )]
	public class CardSpriteSheetInspector : Editor {

		private ReorderableList list;
		private Editor preview;

		private void OnEnable() {
			list = new ReorderableList( serializedObject, serializedObject.FindProperty( "cards" ), true, false, true, true );
			list.drawElementCallback += OnDrawElement;
			list.onSelectCallback += OnSelectElement;
		}

		private void OnDisable() {
			list.drawElementCallback -= OnDrawElement;
			list.onSelectCallback -= OnSelectElement;
		}

		private void OnDrawElement( Rect rect, int index, bool active, bool focused ) {
			var property = list.serializedProperty.GetArrayElementAtIndex( index );
			EditorGUI.PropertyField( rect, property );
		}

		private void OnSelectElement( ReorderableList list ) {
			var sprite = list.serializedProperty.GetArrayElementAtIndex( list.index ).objectReferenceValue as Sprite;
			preview = CreateEditor( sprite );
		}

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();

			list.DoLayoutList();

			serializedObject.ApplyModifiedProperties();
		}

		public override bool HasPreviewGUI() {
			return preview != null;
		}

		public override void OnPreviewGUI( Rect r, GUIStyle background ) {
			preview.OnPreviewGUI( r, background );
		}
	}
}