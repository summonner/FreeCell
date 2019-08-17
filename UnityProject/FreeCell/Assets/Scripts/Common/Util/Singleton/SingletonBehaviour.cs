using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.Util.Singleton {
	public abstract class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T> {
		private static T _instance = null;
		protected static T instance {
			get {
				if ( _instance == null ) {
					_instance = FindInstance();
				}
				return _instance;
			}
		}
		
		private static readonly string alreadyExist = "Another instance already exists";
		private static T FindInstance() {
			var found = FindObjectFromScene();
			if ( found.IsNullOrEmpty() == true ) {
				return null;
			}
			else if ( found.Count == 1 ) {
				return found[0];
			}
			else {
				Debug.LogError( alreadyExist, found[1] );
				throw new System.Exception( alreadyExist );
			}
		}

		private static IList<T> FindObjectFromScene() {
			var founds = new List<T>();
			var rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
			foreach ( var rootObject in rootObjects ) {
				founds.AddRange( rootObject.GetComponentsInChildren<T>( true ) );
			}
			return founds;
		}

		protected T CreateIntance() {
			var gameObject = new GameObject( typeof( T ).Name );
			gameObject.transform.Reset();
			return gameObject.AddComponent<T>();
		}

		protected virtual void OnEnable() {
			if ( instance != this ) {
				Debug.LogError( alreadyExist + " : " + instance, instance );
				throw new System.Exception( alreadyExist );
			}
		}
	}
}