using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;

namespace Summoner.Util.PlayerPrefs {
	[CustomEditor( typeof(PlayerPrefsEditor) )]
	public class PlayerPrefsEditorInspector : Editor {
		private SimpleReorderableList list;
		void OnEnable() {
			list = new SimpleReorderableList( serializedObject, "values" );
		}

		void OnDisable() {
			list?.Dispose();
		}

		public override void OnInspectorGUI() {
			if ( list == null ) {
				return;
			}

			list.DoLayoutList();

			using ( new EditorGUILayout.HorizontalScope() ) {
				EditorGUILayout.Space();
				EditorGUILayout.Space();
				if ( GUILayout.Button( "Delete All" ) == true ) {
					UnityEngine.PlayerPrefs.DeleteAll();
				}
			}
		}
	}
}