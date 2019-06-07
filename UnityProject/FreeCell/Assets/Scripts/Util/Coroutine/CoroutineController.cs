using UnityEngine;
using System.Collections;

namespace Summoner.Util.Coroutine {
	public class CoroutineController : IEnumerator {
		private readonly IEnumerator routine;
		public CoroutineController( IEnumerator routine ) {
			Debug.Assert( routine != null );
			this.routine = routine;
		}

		private enum State {
			Initialized,
			Running,
			Finished,
		};
		private State state = State.Initialized;
		public bool pause { get; set; }

		public bool isRunning {
			get {
				return state == State.Running;
			}
		}

		object IEnumerator.Current {
			get {
				if ( pause == true ) {
					return null;
				}

				if ( state == State.Initialized ) {
					return null;
				}

				return routine.Current;
			}
		}

		bool IEnumerator.MoveNext() {
			if ( state == State.Finished ) { 
				return false;
			}

			if ( pause == true ) {
				return true;
			}

			var moveNext = routine.MoveNext();
			if ( moveNext == false ) {
				Stop();
			}
			else {
				state = State.Running;
			}

			return moveNext;
		}

		public void Reset() {
			routine.Reset();
		}

		public void Stop() {
			state = State.Finished;
		}


		public readonly static CoroutineController Emptied = new CoroutineController( DoNothing() );
		private static IEnumerator DoNothing() {
			yield break;
		}
	}
}