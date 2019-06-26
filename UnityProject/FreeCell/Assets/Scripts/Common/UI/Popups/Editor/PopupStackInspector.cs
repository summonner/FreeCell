using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Summoner.UI.Popups {
	[CustomEditor( typeof(PopupStack) )]
	public class PopupStackInspector : Editor {
		private bool showStack = false;
		public override void OnInspectorGUI() {
			base.OnInspectorGUI();

			var stack = ((PopupStack)target).GetStack();
			showStack = EditorGUILayout.Foldout( showStack, "Popup Stack" );
			if ( showStack == false ) {
				return;
			}

			GUI.enabled = false;
			using ( new EditorGUI.IndentLevelScope() ) {
				EditorGUILayout.IntField( "Count", stack.Count );
				EditorGUILayout.LabelField( "Top" );
				foreach ( var element in stack ) {
					var asComponent = element as Component;
					if ( asComponent == null ) {
						EditorGUILayout.LabelField( element.ToString() );
					}
					else {
						EditorGUILayout.ObjectField( asComponent, asComponent.GetType(), true );
					}
				}
				EditorGUILayout.LabelField( "Bottom" );
				GUI.enabled = true;
			}
		}
	}
}