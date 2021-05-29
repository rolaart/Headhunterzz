using System.Linq;
using Settings;
using UnityEngine;

namespace World.WorldGeneration {

	public static class BiomeGeneration {
		public static Biome Generate(int x, int y, BiomeSettings biomeSettings) {
			float temperature = Noise.GenerateNoise(x, y, biomeSettings.temperatureNoiseSettings);
			float humidity = Noise.GenerateNoise(x, y, biomeSettings.humidityNoiseSettings);
			
			Debug.Log("Humidity: " + humidity + " Temperature: " + temperature);
			Biome biome = biomeSettings.biomes.FirstOrDefault(biome => temperature <= biome.maxTemperature && humidity <= biome.maxHumidity);

			return biome ?? biomeSettings.biomes[2];
		}
	}

}