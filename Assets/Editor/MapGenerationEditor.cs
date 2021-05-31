using UnityEditor;
using UnityEngine;
using World;

namespace Editor {

	[CustomEditor(typeof(Map))]
	public class MapEditor : UnityEditor.Editor {
		public override void OnInspectorGUI() {
			Map mapGen = (Map)target;
			base.OnInspectorGUI();
			// Default Button for Generating New Map, based on the settings of Noise and Height values
			if (GUILayout.Button("Generate")) {
				mapGen.Generate();
			}
		}
	}

}