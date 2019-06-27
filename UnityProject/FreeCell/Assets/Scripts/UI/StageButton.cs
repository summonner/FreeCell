using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
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
		public PresentInt presentStageNumber;
		private IContents info;

		void Reset() {
			symbol = GetComponentInChildren<SVGImageEx>();
			anim = GetComponentInChildren<TweenScale>();
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
			var random = new System.Random( info.stageNumber.GetHashCode() );
			
			this.info = info;
			presentStageNumber.Invoke( info.stageNumber );
			ShowSymbol( random, info.isCleared );
		}

		private void ShowSymbol( System.Random random, bool isCleared ) {
			var index = (int)(random.NextDouble() * sprites.Length);
			var degree = (float)(random.NextDouble() * 360);
			symbol.sprite = sprites.ElementAtOrDefault( index ); ;
			symbol.rectTransform.rotation = Quaternion.Euler( 0, 0, degree );
			symbol.SetNativeSize();

			symbol.color = isCleared ? new Color( 1f, 1f, 1f, 0.4f ) : new Color( 0f, 0f, 0f, 0f );
		}

		private void OnClick() {
			info.OnClick();
		}

		public Coroutine PlayClearAnim() {
			symbol.color = new Color( 1f, 1f, 1f, 0.4f );
			return anim.Play();
		}
	}
}