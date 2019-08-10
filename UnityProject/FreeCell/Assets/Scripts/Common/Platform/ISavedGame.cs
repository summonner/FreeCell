using UnityEngine;
using System.Threading.Tasks;

namespace Summoner.Platform {
	public interface ISavedGame {
		Task SaveAsync( byte[] data );
		Task<byte[]> LoadAsync();
	}
}