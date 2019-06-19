using UnityEngine;
using System.Collections;
using Summoner.Util.Extension;

namespace Summoner.FreeCell {
	public class NotEnoughFreeCellEffect : MonoBehaviour {
		[SerializeField] private Material sprite = null;
		[SerializeField] private AnimationCurve alphaAnim = null;
		[SerializeField] private int numLoops = 1;

		void OnEnable() {
			InGameEvents.OnNotEnoughFreeCells += Activate;
		}

		void OnDisable() {
			InGameEvents.OnNotEnoughFreeCells -= Activate;
		}

		private void Activate() {
			StopAllCoroutines();
			StartCoroutine( Play() );
		}

		private IEnumerator Play() {
			for ( int i=0; i < numLoops; ++i ) {
				foreach ( var t in alphaAnim.EvaluateWithTime() ) {
					SetAlpha( t );
					yield return null;
				}
			}
		}

		private void SetAlpha( float value ) {
			var color = sprite.color;
			color.a = value;
			sprite.color = color;
		}
	}
}