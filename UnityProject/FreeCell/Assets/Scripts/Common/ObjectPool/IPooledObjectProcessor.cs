using UnityEngine;

namespace Summoner.ObjectPool {
	public interface IPooledObjectProcessor<T> {
		void Init( T item );
		void Disable( T item );
	}

	public class OnOff<T> : IPooledObjectProcessor<T> where T : Component {
		public void Init( T item ) {
			item.gameObject.SetActive( true );
		}

		public void Disable( T item ) {
			item.gameObject.SetActive( false );
		}
	}

	public class Store<T> : IPooledObjectProcessor<T> where T : Component {
		private readonly Transform storage;
		public Store( Transform storage ) {
			this.storage = storage;
		}

		public void Init( T item ) {
			item.transform.parent = storage;
		}

		public void Disable( T item ) {
			item.transform.parent = null;
		}
	}
}