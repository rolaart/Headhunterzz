using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Settings {

	public class UpdatableSettings : ScriptableObject {
		/** Required to call from OnValidate method in the class object that will be updated */
		public event System.Action OnValuesUpdated;
		/** Should do AutoUpdate when changing a value in the inspector from the editor */
		public bool autoUpdate;

#if UNITY_EDITOR

		protected virtual void OnValidate() {
			if (autoUpdate) {
				UnityEditor.EditorApplication.update += NotifyOfUpdatedValues;
			}
		}

		public void NotifyOfUpdatedValues() {
			UnityEditor.EditorApplication.update -= NotifyOfUpdatedValues;
			if (OnValuesUpdated != null) {
				OnValuesUpdated();
			}
		}

#endif
	}

}