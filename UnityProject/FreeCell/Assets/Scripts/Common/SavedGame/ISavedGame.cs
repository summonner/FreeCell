using UnityEngine;
using System.Threading.Tasks;

namespace Summoner.SavedGame {
	public interface ISavedGame {
		Task SaveAsync( byte[] data );
		Task<byte[]> LoadAsync();
	}
}