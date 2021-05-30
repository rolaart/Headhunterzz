using System;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Tilemaps;
using World.WorldGeneration;

namespace Settings {

	[CreateAssetMenu]
	public class BiomeSettings : UpdatableSettings {
		public NoiseSettings temperatureNoiseSettings;
		public NoiseSettings humidityNoiseSettings;
		public Biome[] biomes;
	}

}