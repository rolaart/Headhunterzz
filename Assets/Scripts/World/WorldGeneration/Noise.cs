using System;
using Settings;
using UnityEngine;

namespace World.WorldGeneration {

	public static class Noise {
		/** Generates Perlin Noise based on the settings specified through the Editor.
		 *  
		 */
		public static float[,] GenerateMap(int startX, int startY, int width, int height, NoiseSettings settings) {
			float[,] map = new float[width, height];

			// Using the seed for replicating maps
			System.Random random = new System.Random(settings.seed);
			Vector2[] octaveOffsets = new Vector2[settings.octaves];

			for (int i = 0; i < settings.octaves; i++) {
				float offsetX = random.Next(-100000, 100000) + settings.offset.x;
				float offsetY = random.Next(-100000, 100000) - settings.offset.y;
				octaveOffsets[i] = new Vector2(offsetX, offsetY);
			}

			// for normalizing the noise map
			float maxNoiseHeight = float.MinValue;
			float minNoiseHeight = float.MaxValue;

			// Used so when we do changes on the @param scale, it will scale and remain in the center 
			float halfWidth = width / 2f;
			float halfHeight = height / 2f;

			// filling the matrix with values created from the Perlin function
			for (int y = 0; y < width; y++) {
				for (int x = 0; x < height; x++) {
					float amplitude = 1;
					float frequency = 1;
					float noiseHeight = 0;

					// Octaves can be looked as number of samples for a point
					for (int i = 0; i < settings.octaves; i++) {
						float sampleX = (startX + x - halfWidth + octaveOffsets[i].x) / settings.scale * frequency;
						float sampleY = (startY + y - halfHeight + octaveOffsets[i].y) / settings.scale * frequency;

						// changing the range from [-1,1] to [0,1]
						float perlinValue = (Mathf.PerlinNoise(sampleX, sampleY) + 1) / 2;
						noiseHeight += perlinValue * amplitude;

						amplitude *= settings.persistence;
						frequency *= settings.lacunarity;
					}

					maxNoiseHeight = Mathf.Max(maxNoiseHeight, noiseHeight);
					minNoiseHeight = Mathf.Min(minNoiseHeight, noiseHeight);

					map[x, y] = noiseHeight;
				}
			}

			// Normalizing
			for (int y = 0; y < width; y++) {
				for (int x = 0; x < height; x++) {
					map[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, map[x, y]);
				}
			}


			return map;
		}

		/** Generates noise for a single point, based on the noise settings */
		public static float GenerateNoise(int startX, int startY, NoiseSettings settings) {
			// Using the seed for replicating maps
			System.Random random = new System.Random(settings.seed);
			Vector2[] octaveOffsets = new Vector2[settings.octaves];

			for (int i = 0; i < settings.octaves; i++) {
				float offsetX = random.Next(-100000, 100000) + settings.offset.x;
				float offsetY = random.Next(-100000, 100000) - settings.offset.y;
				octaveOffsets[i] = new Vector2(offsetX, offsetY);
			}

			// Used so when we do changes on the @param scale, it will scale and remain in the center 
			float halfWidth = 0.5f;
			float halfHeight = 0.5f;

			float amplitude = 1;
			float frequency = 1;
			float noiseHeight = 0;

			// Octaves can be looked as number of samples for a point
			for (int i = 0; i < settings.octaves; i++) {
				float sampleX = (startX - halfWidth + octaveOffsets[i].x) / settings.scale * frequency;
				float sampleY = (startY - halfHeight + octaveOffsets[i].y) / settings.scale * frequency;

				// changing the range from [-1,1] to [0,1]
				float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);
				noiseHeight += perlinValue * amplitude;

				amplitude *= settings.persistence;
				frequency *= settings.lacunarity;
			}
			
			return noiseHeight % 1;
		}
	}

}