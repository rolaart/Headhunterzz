using System;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;


namespace World.WorldGeneration {

	[Serializable]
	public class Biome {
		[OptionalField] public string name;
		
		[Range(0, 1)] public float maxTemperature;
		
		[Range(0, 1)] public float maxHumidity;

		// the basic tile with no decorations
		public Tile baseTile;
		// all possible tiles for that biome
		public Tile[] tiles;

		public TileBase GetRandomTile() {
			return tiles[Random.Range(0, tiles.Length)];
		}
	}

}