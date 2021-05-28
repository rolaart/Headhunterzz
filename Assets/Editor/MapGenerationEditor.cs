using UnityEditor;
using UnityEngine;
using World.WorldGeneration;

namespace Editor {

	[CustomEditor(typeof(MapGeneration))]
	public class MapGenerationEditor : UnityEditor.Editor {
		public override void OnInspectorGUI() {
			// MapGeneration mapGen = (MapGeneration) target;
			// base.OnInspectorGUI();
			// // Default Button for Generating New Map, based on the settings of Noise and Height values
			// if (GUILayout.Button("Generate")) {
			// 	mapGen.Generate();
			// }
		}
	}

}