using Settings;
using UnityEngine;

namespace World.WorldGeneration {

	public static class HeightMapGeneration {
		public static HeightMap Generate(int startX, int startY, int width, int height, HeightMapSettings settings) {
			float[,] values = Noise.GenerateMap(startX, startY, width, height, settings.noiseSettings);

			AnimationCurve heightCurve = new AnimationCurve(settings.heightCurve.keys);

			// normalization
			float minValue = float.MaxValue;
			float maxValue = float.MinValue;

			for (int i = 0; i < width; i++) {
				for (int j = 0; j < height; j++) {
					values[i, j] *= heightCurve.Evaluate(values[i, j]);

					if (values[i, j] > maxValue) {
						maxValue = values[i, j];
					}

					if (values[i, j] < minValue) {
						minValue = values[i, j];
					}
				}
			}

			return new HeightMap(values, minValue, maxValue);
		}
	}

}