using UnityEngine;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.Util.Singleton {
	public abstract class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T> {
		private static T _instance = null;
		protected static T instance {
			get {
				if ( _instance == null ) {
					_instance = FindOrCreateInstance();
				}
				return _instance;
			}
		}
		
		private static readonly string alreadyExist = "Another instance already exists";
		private static T FindOrCreateInstance() {
			var found = FindObjectsOfType<T>();
			if ( found.IsNullOrEmpty() == true ) {
				var gameObject = new GameObject( typeof(T).Name );
				gameObject.transform.Reset();
				return gameObject.AddComponent<T>();
			}
			else if ( found.Length == 1 ) {
				return found[0];
			}
			else {
				Debug.LogError( alreadyExist, found[1] );
				throw new System.Exception( alreadyExist );
			}
		}

		protected virtual void OnEnable() {
			if ( instance != this ) {
				Debug.LogError( alreadyExist + " : " + instance, instance );
				throw new System.Exception( alreadyExist );
			}
		}
	}
}