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
		public Layer[] layers;
	}

}