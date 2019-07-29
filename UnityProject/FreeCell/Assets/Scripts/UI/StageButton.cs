using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using Summoner.UI;
using Summoner.UI.Tween;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	[SelectionBase]
	public class StageButton : MonoBehaviour {
		public interface IContents {
			StageNumber stageNumber { get; }
			bool isCleared { get; }
			void OnClick();
		}

		[SerializeField] private SVGImageEx symbol = null;
		[SerializeField] private Sprite[] sprites = null;
		[SerializeField] private TweenScale anim = null;
		[SerializeField] private TweenAlpha alpha = null;
		public PresentInt presentStageNumber;
		private IContents info;
		private bool doesReadyForAnim;

		void Reset() {
			symbol = GetComponentInChildren<SVGImageEx>();
			anim = GetComponentInChildren<TweenScale>();
			alpha = GetComponentInChildren<TweenAlpha>();
		}

		void Awake() {
			var button = gameObject.GetOrAddComponent<Button>();
			button.transition = Selectable.Transition.None;
			button.onClick.AddListener( OnClick );
		}

		public void Set( IContents info ) {
#if UNITY_EDITOR
			name = info.stageNumber.ToString();
#endif
			var random = CalculateRandom( info );
			var index = random % sprites.Length;
			var degree = random / 256f * 360f;

			this.info = info;
			presentStageNumber.Invoke( info.stageNumber );
			ShowSymbol( index, degree, info.isCleared );
		}

		private void ShowSymbol( int index, float degree, bool isCleared ) {
			symbol.sprite = sprites.ElementAtOrDefault( index );
			symbol.rectTransform.rotation = Quaternion.Euler( 0, 0, degree );
			symbol.SetNativeSize();

			if ( doesReadyForAnim == false ) {
				anim.value = 1;
				alpha.value = isCleared ? 1 : 0;
			}
		}

		private void OnClick() {
			info.OnClick();
		}

		public void ReadyForClearAnim() {
			anim.value = 0;
			alpha.value = 0;
			doesReadyForAnim = true;
		}

		public Coroutine PlayClearAnim() {
			return StartCoroutine( PlayClearAnimAux() );
		}

		private IEnumerator PlayClearAnimAux() {
			alpha.Play();
			yield return anim.Play();
			doesReadyForAnim = false;
		}

		private int CalculateRandom( IContents info ) {
			var rect = GetComponent<RectTransform>();
			var x = GetMantissa( rect.localPosition.x * info.stageNumber.index );
			var y = GetMantissa( rect.localPosition.y );
			var value = x * y;
			var reversed = 0;
			var length = 32;
			for ( int i=0; i < length; ++i ) {
				reversed |= ((value >> i) & 1) << (length - 1 - i);
			}
			
			if ( reversed < 0 ) {
				reversed *= -1;
			}
			return reversed;
		}

		private int GetMantissa( float x ) {
			var bytes = System.BitConverter.GetBytes( x );
			return System.BitConverter.ToInt32( bytes, 0 ) & 0x007fffff;
		}
	}
}