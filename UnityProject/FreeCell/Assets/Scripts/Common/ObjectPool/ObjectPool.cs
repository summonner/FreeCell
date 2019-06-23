using UnityEngine;
using System.Collections.Generic;
using Summoner.Util.Extension;
using Summoner.ObjectPool;

namespace Summoner {
	public class ObjectPool<T> where T : Component {
		private readonly Stack<T> pool;
		private readonly IPooledObjectProcessor<T> processor;
		public delegate T CreateFunc();
		private readonly CreateFunc Create;
		
		public ObjectPool( CreateFunc onCreateObject ) 
			: this( 0, onCreateObject ) { }

		public ObjectPool( int capacity, CreateFunc onCreateObject ) 
			: this( capacity, onCreateObject, new OnOff<T>() ) { }

		public ObjectPool( CreateFunc onCreateObject, Transform storageNode ) 
			: this( 0, onCreateObject, storageNode ) { }

		public ObjectPool( int capacity, CreateFunc onCreateObject, Transform storageNode )
			: this( capacity, onCreateObject, new OnOff<T>(), new Store<T>( storageNode ) ) { }

		public ObjectPool( int capacity, CreateFunc onCreateObject, params IPooledObjectProcessor<T>[] processors ) {
			this.pool = new Stack<T>( capacity );
			this.processor = Flat( processors );
			this.Create = onCreateObject;
			Debug.Assert( Create != null, "onCreateObject cannot be null" );
		}

		private static IPooledObjectProcessor<T> Flat( IList<IPooledObjectProcessor<T>> processors ) {
			if ( processors.IsNullOrEmpty() == true ) {
				return new OnOff<T>();
			}
			else if ( processors.Count == 1 ) {
				return processors[0];
			}
			else {
				return new ProcessorChain( processors );
			}
		}

		public void Push( T item ) {
			processor.Disable( item );
			pool.Push( item );
		}

		public T Pop() {
			var item = GetItem();
			processor.Init( item );
			return item;
		}

		private T GetItem() {
			if ( pool.Count > 0 ) {
				return pool.Pop();
			}

			return Create();
		}

		public void Clear() {
			pool.Clear();
		}


		private class ProcessorChain : IPooledObjectProcessor<T> {
			private readonly IList<IPooledObjectProcessor<T>> processors;
			public ProcessorChain( IList<IPooledObjectProcessor<T>> processors ) {
				this.processors = processors;
			}

			public void Init( T item ) {
				foreach ( var processor in processors ) {
					processor.Init( item );
				}
			}

			public void Disable( T item ) {
				foreach ( var processor in processors ) {
					processor.Disable( item );
				}
			}
		}
	}
}