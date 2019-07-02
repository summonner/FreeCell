using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Summoner.Util.Extension;
using Summoner.Util.Singleton;

namespace Summoner.FreeCell {

	public class SoundPlayer : SingletonBehaviour<SoundPlayer> {
		[SerializeField] private SoundList[] lists = null;
		
		public static SoundPlayer Instance {
			get {
				return instance;
			}
		}

		public void Play( SoundType sound ) {
			Play( sound, 1f );
		}

		public void Play( SoundType sound, float volume ) {
			var list = lists.FirstOrDefault( (item) => ( item.type == sound ) );
			if ( list.source == null ) {
				return;
			}

			var playFunc = GetMethod( list.method );
			playFunc( list.source, list.clips, volume );
		}

		private delegate void PlayFunc( AudioSource source, IList<AudioClip> clips, float volume );
		private static PlayFunc GetMethod( PlayMethod method ) {
			switch ( method ) {
				case PlayMethod.All:
					return PlayAll;
				case PlayMethod.Random:
					return PlayRandom;
				default:
					Debug.LogError( "Unknown PlayMethod type : " + method );
					return delegate { };
			}
		}

		private static void PlayAll( AudioSource source, IList<AudioClip> clips, float volume ) {
			foreach ( var clip in clips ) {
				source.PlayOneShot( clip, volume );
			}
		}

		private static void PlayRandom( AudioSource source, IList<AudioClip> clips, float volume ) {
			var clip = SelectRandom( clips );
			source.PlayOneShot( clip, volume );
		}

		private static AudioClip SelectRandom( IList<AudioClip> clips ) {
			if ( clips.IsNullOrEmpty() == true ) {
				return null;
			}

			if ( clips.Count == 1 ) {
				return clips[0];
			}

			var selected = Random.Range( 0, clips.Count );
			return clips[selected];
		}


		private enum PlayMethod {
			Random,
			All,
		}

#pragma warning disable 0649
		[System.Serializable]
		private struct SoundList {
			[SerializeField] private SoundType _type;
			public SoundType type {
				get {
					return _type;
				}
			}

			[SerializeField] private PlayMethod _method;
			public PlayMethod method {
				get {
					return _method;
				}
			}

			[SerializeField] private AudioSource _source;
			public AudioSource source {
				get {
					return _source;
				}
			}

			[SerializeField] private AudioClip[] _clips;
			public IList<AudioClip> clips {
				get {
					return _clips.AsReadOnly();
				}
			}
		}
#pragma warning restore 0649
	}
}