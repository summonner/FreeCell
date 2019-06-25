using UnityEngine;
using System.Collections.Generic;

namespace Summoner.Util.Singleton {
	public abstract class Singleton<T> : System.IDisposable where T : Singleton<T> {
		protected static T instance { get; private set; }

		protected Singleton() {
			if ( instance != null ) {
				throw new System.Exception( "Another instance already exists" );
			}

			instance = (T)this;
		}

		public virtual void Dispose() {
			instance = null;
		}
	}
}