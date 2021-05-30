using System;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;
using World.WorldGeneration;

namespace Settings {

	/** Representing a level of tiles that can appear on based height */
	[Serializable]
	public class Layer {
		/** The Tile Asset */
		public Tile tile;

		/** The Upper Bound of where it can appear */
		[Range(0, 1)] public float maxHeight;
	}

	/** Functionality to add tiles from the editor */
	[CreateAssetMenu]
	public class TileSettings : UpdatableSettings {
		public Layer waterLayer;
		public Layer groundLayer;
		public Layer borderLayer;

		public Tile GetLayerTile(Biome biome, float height) {
			Assert.IsTrue(height >= 0 && height <= 1, "Invalid height: " + height);
			if (height <= waterLayer.maxHeight) {
				return waterLayer.tile;
			}

			if (height <= groundLayer.maxHeight) {
				return biome.baseTile;
			}

			if (height <= borderLayer.maxHeight) {
				return borderLayer.tile;
			}
			
			// we shouldn't get here
			Assert.IsTrue(2 < 1, "Height is: " + height);
			return null;
		}

#if UNITY_EDITOR

		protected override void OnValidate() {
			// not letting the min height of every next layer to be less than the max height of
			// the previous and it's max to be not less than the min
			groundLayer.maxHeight = Mathf.Max(waterLayer.maxHeight, groundLayer.maxHeight);
			borderLayer.maxHeight = Mathf.Max(groundLayer.maxHeight, borderLayer.maxHeight);
			base.OnValidate();
		}
#endif
	}

}