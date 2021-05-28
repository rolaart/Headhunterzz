using System;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Settings {

	/** Representing a level of tiles that can appear on based height */
	[Serializable]
	public class Layer {
		/** Convenient field for representing the tile, not used for anything */
		[OptionalField] public string name;

		/** The Tile Asset */
		public Tile tile;

		/** The Lower Bound of where it can appear */
		[Range(0, 1)] public float minHeight;

		/** The Upper Bound of where it can appear */
		[Range(0, 1)] public float maxHeight;
	}

	/** Functionality to add tiles from the editor */
	[CreateAssetMenu]
	public class TileSettings : UpdatableSettings {
		/** scaling the interval [0, 1] to [0, 10] so we can have O(1) access to the
		 * layer that should be to the corresponding height
		 */
		private const int Scale = 10;
		private static readonly int[] Intervals = new int[Scale];
		
		public Layer[] layers;
		
		public Tile GetLayerTile(float height) {
			return layers[Intervals[Mathf.FloorToInt(height * (Scale - 1))]].tile;
		}

#if UNITY_EDITOR

		protected override void OnValidate() {
			// not letting the min height of every next layer to be less than the max height of
			// the previous and it's max to be not less than the min
			for (int i = 1; i < layers.Length; i++) {
				layers[i].minHeight = layers[i - 1].maxHeight;
				layers[i].maxHeight = Mathf.Max(layers[i].minHeight, layers[i].maxHeight);
			}

			// Update the interval array for tiles
			for (int i = 0; i < layers.Length; i++) {
				for (int j = Mathf.FloorToInt(layers[i].minHeight * Scale); j < Mathf.CeilToInt(layers[i].maxHeight * Scale); j++) {
					Intervals[j] = i;
					// Debug.Log(j + ", " + Intervals[j]);
				}
			}
			base.OnValidate();
		}
#endif
	}

}