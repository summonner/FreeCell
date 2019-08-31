using UnityEngine;
using System.Collections.Generic;
using Summoner.Platform;
using Summoner.Util.Singleton;

namespace Summoner.FreeCell {
	public class Platform : Singleton<Platform> {
		private IPlatform platform;
		public static IPlatform Instance {
			get {
				if ( instance == null ) {
					new Platform();
				}

				return instance.platform;
			}
		}

		public Platform() {
			this.platform = CreatePlatform();
		}

		private static IPlatform CreatePlatform() {
#if UNITY_EDITOR
			return new OfflineProxy( 1f, true );
#elif UNITY_ANDROID
			return new GooglePlayGamesService( GooglePlayGamesService.Option.SavedGame
										//	 | GooglePlayGamesService.Option.Debug
											 );
#endif
		}
	}
}