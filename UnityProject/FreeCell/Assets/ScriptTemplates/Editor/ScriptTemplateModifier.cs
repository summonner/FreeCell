//ref : https://forum.unity.com/threads/c-script-template-how-to-make-custom-changes.273191/#post-1806467

using UnityEngine;
using UnityEditor;
using Summoner.Util.Extension;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Summoner.Util.ScriptTemplates {

	public class KeywordReplace : UnityEditor.AssetModificationProcessor {

		private static string ReplaceContent( string content, string path ) {
			content = content.Replace( "#PROJECTNAME#", GetProjectName( path ) );
			content = content.Replace( "#NAMESPACE#", ToNameSpace( path ) );
			content = content.Replace( "#TARGETNAME#", GetTargetName( path ) );
			return content;
		}

		private static string GetProjectName( string path ) {
			if ( path.Contains( "Common" ) == true ) {
				return ToNameSpace( path );
			}
			else {
				return PlayerSettings.productName;
			}
		}

		private const char nameSpaceSeparator = '.';
		private static string ToNameSpace( string path ) {
			path = Path.GetDirectoryName( path );
			const string toFind = "Scripts";
			var index = path.LastIndexOf( toFind );
			path = path.Substring( index + toFind.Length );
			path = path.Replace( Path.DirectorySeparatorChar, nameSpaceSeparator );
			path = path.Replace( Path.AltDirectorySeparatorChar, nameSpaceSeparator );
			path = Remove( path, ".Common", ".Editor" );
			path = path.Trim( nameSpaceSeparator );
			return path;
		}

		private static string Remove( string path, params string[] values ) {
			foreach ( var toRemove in values ) {
				var pattern = string.Format( @"{0}\b", Regex.Escape( toRemove ) );
				path = Regex.Replace( path, pattern, "" );
			}

			return path;
		}

		private static string GetTargetName( string path ) {
			var fileName = System.IO.Path.GetFileNameWithoutExtension( path );
			return Remove( fileName, "Inspector", "Editor", "Drawer" );
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