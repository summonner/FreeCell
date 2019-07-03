using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Summoner.Util.Extension;
using Summoner.EditorExtensions;
using UnityPrefs = UnityEngine.PlayerPrefs;
using Type = Summoner.Util.PlayerPrefs.PlayerPrefsEditor.Type;

namespace Summoner.Util.PlayerPrefs {
	[CustomPropertyDrawer( typeof(PlayerPrefsEditor.PreferenceValue) )]
	public class PreferenceDrawer : PropertyDrawer {
		public override void OnGUI( Rect position, SerializedProperty property, GUIContent label ) {
			var layout = new PropertyLayoutHelper();
			var key = property.FindPropertyRelative( "key" );
			var type = property.FindPropertyRelative( "type" );

			position = PropertyLayoutHelper.AdjustRect( position, EditorGUIUtility.singleLineHeight );

			layout.Begin();
			layout.Add( (rect) => { EditorGUI.PropertyField( rect, type, GUIContent.none ); }, 40 );
			layout.Add( (rect) => { EditorGUI.PropertyField( rect, key, GUIContent.none ); }, 150 );
			layout.Add( DrawValue( key.stringValue, type ) );
			layout.Add( DrawButton( key.stringValue, type ), 20 );
			layout.End();
			layout.Render( position );
		}

		private PropertyLayoutHelper.RenderFunc DrawValue( string key, SerializedProperty type ) {
			return delegate ( Rect rect ) {
				if ( UnityPrefs.HasKey( key ) == false ) {
					EditorGUI.LabelField( rect, "Cannot find value" );
					return;
				}

				drawers[(Type)type.intValue].Draw( rect, key );
			};
		}

		private void Draw<T>( System.Func<Rect, T, T> DrawField, Rect rect, System.Func<string, T> Get, System.Action<string, T> Set, string key ) {
			var value = Get( key );
			var newValue = DrawField( rect, value );
			if ( value.Equals( newValue ) ) {
				return;
			}

			Set( key, newValue );
			UnityPrefs.Save();
		}

		private PropertyLayoutHelper.RenderFunc DrawButton( string key, SerializedProperty type ) {
			if ( key.IsNullOrEmpty() == true ) {
				return delegate { };
			}

			return delegate ( Rect rect ) {
				if ( UnityPrefs.HasKey( key ) == true ) {
					if ( GUI.Button( rect, "X" ) == true ) {
						UnityPrefs.DeleteKey( key );
					}
				}
				else {
					if ( GUI.Button( rect, "+" ) == true ) {
						drawers[(Type)type.intValue].Add( key );
					}
				}
			};
		}

		private interface IValueDrawer {
			void Add( string key );
			void Draw( Rect rect, string key );
		}

		private static readonly IDictionary<Type, IValueDrawer> drawers = new Dictionary<Type, IValueDrawer>() {
			{ Type.Int, new ValueDrawer<int>( EditorGUI.IntField, UnityPrefs.GetInt, UnityPrefs.SetInt ) },
			{ Type.Float, new ValueDrawer<float>( EditorGUI.FloatField, UnityPrefs.GetFloat, UnityPrefs.SetFloat ) },
			{ Type.String, new ValueDrawer<string>( EditorGUI.TextField, UnityPrefs.GetString, UnityPrefs.SetString ) },
		};

		private class ValueDrawer<T> : IValueDrawer {
			private readonly System.Func<Rect, T, T> DrawField;
			private readonly System.Func<string, T> Get;
			private readonly System.Action<string, T> Set;

			public ValueDrawer( System.Func<Rect, T, T> DrawField, System.Func<string, T> Get, System.Action<string, T> Set ) {
				this.DrawField = DrawField;
				this.Get = Get;
				this.Set = Set;
			}

			public void Add( string key ) {
				Set( key, default(T) );
			}

			public void Draw( Rect rect, string key ) {
				var value = Get( key );
				var newValue = DrawField( rect, value );
				if ( value.Equals( newValue ) ) {
					return;
				}

				Set( key, newValue );
				UnityPrefs.Save();
			}
		}
	}
}