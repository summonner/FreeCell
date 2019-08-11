using UnityEngine;
using System.Collections;
using System.Threading.Tasks;

// ref : http://www.stevevermeulen.com/index.php/2017/09/using-async-await-in-unity3d-2017/
namespace Summoner {
	public static class TaskExtension {
		public static async void WrapError( this Task task ) {
			await task;
		}

		public static IEnumerator AsIEnumerator( this Task task ) {
			while ( !task.IsCompleted ) {
				yield return null;
			}

			if ( task.IsFaulted ) {
				throw task.Exception;
			}
		}

		public static IEnumerator AsIEnumerator<T>( this Task<T> task, System.Action<T> onComplete ) {
			while ( !task.IsCompleted ) {
				yield return null;
			}

			if ( task.IsFaulted ) {
				throw task.Exception;
			}

			onComplete( task.Result );
		}
	}
}