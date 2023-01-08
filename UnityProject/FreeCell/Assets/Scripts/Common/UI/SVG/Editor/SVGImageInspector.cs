using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Summoner.UI.SVG {
	[CustomEditor( typeof(SVGImageEx) )]
	[CanEditMultipleObjects]
	public class SVGImageInspector : Unity.VectorGraphics.SVGImageEditor {
		public override void OnInspectorGUI() {
			base.OnInspectorGUI();
			if ( m_ShowNativeSize.value == false ) {
				SetShowNativeSize( true, true );
			}
			NativeSizeButtonGUI();
		}
	}
}