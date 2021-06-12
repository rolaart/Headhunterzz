using UnityEngine;

namespace Settings {
	[CreateAssetMenu]
	public class KeybindsSettings : ScriptableObject {
		public KeyCode dashKey;
		public KeyCode attackKey;
		public KeyCode inventoryKey;
	}

}