using UnityEngine;
using System.Collections.Generic;

namespace Summoner.FreeCell.Test {
	public class TestStageRange : System.IDisposable {
		public TestStageRange()
			: this( 1, 1000000 ) { }

		public TestStageRange( int min, int max ) {
			StageInfo.SetRange( min, max );
		}

		public void Dispose() {
			StageInfo.SetRangeAsDefault();
		}
	}
}