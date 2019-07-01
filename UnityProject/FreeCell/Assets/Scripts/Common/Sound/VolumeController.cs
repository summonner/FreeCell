using UnityEngine;
using UnityEngine.Audio;

namespace Summoner.Sound {
	public class VolumeController : MonoBehaviour {
		[SerializeField] private AudioMixer mixer;
		[SerializeField] private string parameterName;
		private ISavedValue saved;
		private float defaultValue;
		
		private const float min = -80f;

		void Start() {
			var key = mixer.name + "." + parameterName;
			mixer.GetFloat( parameterName, out this.defaultValue );
			saved = new PlayerPrefsValue( key, defaultValue );
			Load();
		}

		public void Load() {
			// UNITY_BUG : does not works on Awake() or OnEnable().
			mixer.SetFloat( parameterName, saved.value );
		}

		public float decibel {
			get {
				var value = 0f;
				mixer.GetFloat( parameterName, out value );
				return value;
			}

			set {
				mixer.SetFloat( parameterName, value );
				saved.value = value;
			}
		}

		public float normalizedVolume {
			get {
				return Mathf.Pow( 10, decibel / 20f );
			}

			set {
				if ( value < 0.0001f ) {
					value = 0.0001f;
				}

				decibel = Mathf.Log10( value ) * 20f;
			}
		}

		public void Mute( bool muted ) {
			decibel = (muted ? min : defaultValue);
		}

		public bool isMuted {
			get {
				return decibel <= min;
			}
		}

		private interface ISavedValue {
			float value { get; set; }
		}

		private class PlayerPrefsValue : ISavedValue {
			private readonly string key;
			private readonly float defaultValue;

			public PlayerPrefsValue( string key, float defaultValue ) {
				this.key = key;
				this.defaultValue = defaultValue;
			}

			public float value {
				get {
					return PlayerPrefs.GetFloat( key, defaultValue );
				}

				set {
					PlayerPrefs.SetFloat( key, value );
					PlayerPrefs.Save();
				}
			}
		}
	}
}