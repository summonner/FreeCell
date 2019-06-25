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
		
		private static T FindOrCreateInstance() {
			var found = FindObjectOfType<T>();
			if ( found != null ) {
				return found;
			}

			var gameObject = new GameObject( typeof(T).Name );
			gameObject.transform.Reset();
			return gameObject.AddComponent<T>();
		}
	}
}