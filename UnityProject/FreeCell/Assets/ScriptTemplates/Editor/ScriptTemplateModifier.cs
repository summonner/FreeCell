//ref : https://forum.unity.com/threads/c-script-template-how-to-make-custom-changes.273191/#post-1806467

using UnityEngine;
using UnityEditor;
using Summoner.Util.Extension;
using System.Collections.Generic;

namespace Summoner.Util.ScriptTemplates {

	public class KeywordReplace : UnityEditor.AssetModificationProcessor {

		private static string ReplaceContent( string content, string path ) {
			content = content.Replace( "#PROJECTNAME#", PlayerSettings.productName );
			content = content.Replace( "#NAMESPACE#", AsNameSpace( path ) );
			return content;
		}

		private static string AsNameSpace( string path ) {
			path = System.IO.Path.GetDirectoryName( path );
			const string toFind = "Scripts";
			var index = path.LastIndexOf( toFind );
			path = path.Substring( index + toFind.Length );
			path = path.Replace( '/', '.' );
			return path;
		}

		public static void OnWillCreateAsset( string path ) {
			path = path.Replace( ".meta", "" );

			var file = File.Find( path );
			if ( file == null ) {
				return;
			}

			file.content = ReplaceContent( file.content, path );

			AssetDatabase.Refresh();
		}

		private class File {
			private readonly string path;

			public string content {
				get {
					return System.IO.File.ReadAllText( path );
				}

				set {
					System.IO.File.WriteAllText( path, value );
				}
			}

			private File( string path ) {
				this.path = path;
			}

			private static readonly IList<string> scriptExtensions = new [] { ".cs", ".js", ".boo" };
			private static readonly string dataPath = Application.dataPath.Substring( 0, Application.dataPath.LastIndexOf( "Assets" ) );

			public static File Find( string path ) {
				var extension = System.IO.Path.GetExtension( path );
				if ( extension == ".meta" ) {
					path = path.Replace( ".meta", "" );
					return Find( path );
				}

				if ( extension.IsNullOrEmpty() == true ) {
					return null;
				}

				if ( scriptExtensions.Contains( extension ) == false ) {
					return null;
				}

				path = dataPath + path;
				if ( System.IO.File.Exists( path ) == false ) {
					return null;
				}

				return new File( path );
			}
		}
	}
}