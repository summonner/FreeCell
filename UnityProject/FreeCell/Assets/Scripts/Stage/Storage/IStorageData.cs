using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell {
	public interface IStorageData {
		int Load( int pageIndex );
		void Save( int pageIndex, int values );
	}
}