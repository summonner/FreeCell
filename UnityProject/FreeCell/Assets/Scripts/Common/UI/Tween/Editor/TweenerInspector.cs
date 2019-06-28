using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Summoner.UI.Tween {
	[CustomEditor( typeof(TweenBase), true )]
	public class TweenerInspector : Editor {
		private float value = 0f;
		TweenBase tweener = null;

		private void OnEnable() {
			value = 0;
			tweener = target as TweenBase;
		}

		private void OnDisable() {
			value = 0;
			tweener.value = 0;
		}

		public override void OnInspectorGUI() {
			using ( new EnableScope( Application.isPlaying == false ) ) {
				value = EditorGUILayout.Slider( "Preview", value, 0, 1 );
				if ( Application.isPlaying == false ) {
					tweener.value = value;
				}

				using ( new EditorGUILayout.HorizontalScope() ) {
					if ( GUILayout.Button( "PlayForward" ) == true ) {
						tweener.Play();
					}

					if ( GUILayout.Button( "PlayBackward" ) == true ) {
						tweener.PlayReverse();
					}
				}
			}

			base.OnInspectorGUI();
		}

		private void Update() {

		}
	}
}