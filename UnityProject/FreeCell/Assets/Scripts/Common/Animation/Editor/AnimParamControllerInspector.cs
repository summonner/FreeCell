using UnityEngine;
using UnityEditor;

namespace Summoner.Animation {
	[CustomEditor( typeof(AnimParamController) )]
	public class AnimParamControllerInspector : Editor {
		public override void OnInspectorGUI() {
			var anim = FindAnimator();
			if ( anim == null ) {
				return;
			}

			var allParams = anim.parameters;
			if ( allParams.Length == 0 ) {
				EditorGUILayout.HelpBox( "There is no paramters found", MessageType.Info );
				return;
			}

			var value = serializedObject.FindProperty( "parameter" );
			EditorGUILayout.LabelField( "Parameters" );
			using ( new EditorGUI.IndentLevelScope() ) {
				for ( int i = 0; i < allParams.Length; ++i ) {
					using ( new EditorGUILayout.HorizontalScope() ) {
						var param = allParams[i];
						DrawSelection( value, param );
						DrawParam( anim, param );
					}
				}
			}
			anim.Update( Time.deltaTime );

			serializedObject.ApplyModifiedProperties();
		}

		private Animator FindAnimator() {
			var anim = ((AnimParamController)target).GetComponent<Animator>();

			if ( anim == null ) {
				EditorGUILayout.HelpBox( "Cannot find 'Animator' component", MessageType.Error );
				return null;
			}

			if ( anim.isActiveAndEnabled == false ) {
				EditorGUILayout.HelpBox( "Animator has disabled", MessageType.Info );
				return null;
			}

			if ( anim.runtimeAnimatorController == null ) {
				EditorGUILayout.HelpBox( "Cannot find available 'AnimatorController'", MessageType.Warning );
				return null;
			}

			if ( anim.hasBoundPlayables == false ) {
				anim.Rebind();
			}

			return anim;
		}

		private const float selectionWidth = 20;
		private static void DrawSelection( SerializedProperty value, AnimatorControllerParameter param ) {
			var selected = EditorGUILayout.Toggle( value.intValue == param.nameHash, GUILayout.Width( selectionWidth ) );
			if ( selected == true ) {
				value.intValue = param.nameHash;
			}
		}

		private static void DrawParam( Animator anim, AnimatorControllerParameter param ) {
			switch ( param.type ) {
				case AnimatorControllerParameterType.Bool:
					DrawBool( anim, param );
					break;

				case AnimatorControllerParameterType.Float:
					DrawFloat( anim, param );
					break;

				case AnimatorControllerParameterType.Int:
					DrawInt( anim, param );
					break;

				case AnimatorControllerParameterType.Trigger:
					DrawTrigger( anim, param );
					break;

				default:
					EditorGUILayout.LabelField( "UnknownParameterType : " + param.type );
					break;
			}
		}

		private static void DrawBool( Animator anim, AnimatorControllerParameter param ) {
			var prev = anim.GetBool( param.nameHash );
			var current = EditorGUILayout.Toggle( param.name, prev );
			if ( prev != current ) {
				anim.SetBool( param.nameHash, current );
			}
		}

		private static void DrawFloat( Animator anim, AnimatorControllerParameter param ) {
			var prev = anim.GetFloat( param.nameHash );
			var current = EditorGUILayout.FloatField( param.name, prev );
			if ( prev != current ) {
				anim.SetFloat( param.nameHash, current );
			}
		}

		private static void DrawInt( Animator anim, AnimatorControllerParameter param ) {
			var prev = anim.GetInteger( param.nameHash );
			var current = EditorGUILayout.IntField( param.name, prev );
			if ( prev != current ) {
				anim.SetInteger( param.nameHash, current );
			}
		}

		private static void DrawTrigger( Animator anim, AnimatorControllerParameter param ) {
			EditorGUILayout.LabelField( param.name );
			var pressed = GUILayout.Button( "Set" );
			if ( pressed == true ) {
				anim.SetTrigger( param.nameHash );
			}

			pressed = GUILayout.Button( "Reset" );
			if ( pressed == true ) {
				anim.ResetTrigger( param.nameHash );
			}
		}
	}
}