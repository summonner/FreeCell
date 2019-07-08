using UnityEngine;
using System.Collections.Generic;

namespace Summoner.Util {
	[ExecuteInEditMode]
	public class CameraSizeScaler : MonoBehaviour {
		[SerializeField] private new Camera camera = null;
		[SerializeField] private float targetAspect = 9f/16f;
		[SerializeField] private float baseSize = 8f;

		void Reset() {
			camera = Camera.main;
			targetAspect = camera.aspect;
			baseSize = camera.orthographicSize;
		}

		void OnEnable() {
			SetScale();
		}

		void OnDisable() {
			camera.orthographicSize = baseSize;
		}

		void Update() {
			SetScale();
		}

		private void SetScale() {
			camera.orthographicSize = baseSize * CalculateScale();
		}

		private float CalculateScale() {
			if ( camera.aspect <= targetAspect ) {
				return targetAspect / camera.aspect;
			}
			else {
				return 1f;
			}
		}

	}
}