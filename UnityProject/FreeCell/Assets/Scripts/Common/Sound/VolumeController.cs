using UnityEngine;
using UnityEngine.Audio;
using Summoner.Util;

namespace Summoner.Sound {
	public class VolumeController : MonoBehaviour {
		[SerializeField] private AudioMixer mixer = null;
		[SerializeField] private string parameterName = null;
		private ISavedValue<float> saved;
		private float defaultValue = 0f;
		
		private const float min = -80f;

		void Start() {
			Load();
		}

		public void Load() {
			if ( saved == null ) {
				var key = mixer.name + "." + parameterName;
//				mixer.GetFloat( parameterName, out this.defaultValue );
				saved = PlayerPrefsValue.Float( key, defaultValue );
			}

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
	}
}