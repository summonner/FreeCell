using UnityEngine;
using System.Threading.Tasks;

// ref : http://www.stevevermeulen.com/index.php/2017/09/using-async-await-in-unity3d-2017/
namespace Summoner {
	public static class TaskExtension {
		public static async void WrapError( this Task task ) {
			await task;
		}
	}
}