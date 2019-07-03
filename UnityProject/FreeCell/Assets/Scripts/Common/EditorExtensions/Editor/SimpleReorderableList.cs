using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;

namespace Summoner {
	public class SimpleReorderableList : System.IDisposable {
		private readonly ReorderableList list;
		private readonly SerializedObject serializedObject;
		public SimpleReorderableList( SerializedObject serializedObject, string propertyPath ) {
			this.serializedObject = serializedObject;
			this.list = new ReorderableList( serializedObject, serializedObject.FindProperty( propertyPath ) );
			this.list.drawElementCallback += OnDraw;
		}

		public void Dispose() {
			list.drawElementCallback -= OnDraw;
		}

		private SerializedProperty GetElement( int index ) {
			return list.serializedProperty.GetArrayElementAtIndex( index );
		}

		private void OnDraw( Rect rect, int index, bool isActive, bool isFocused ) {
			var property = GetElement( index );
			EditorGUI.PropertyField( rect, property );
		}

		public void DoLayoutList() {
			list.DoLayoutList();
			serializedObject.ApplyModifiedProperties();
		}
	}
}