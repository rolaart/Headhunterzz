using System;
using UnityEngine;

namespace Settings {

	[Serializable]
	public class NoiseSettings {
		[Range(1, 50)] public float scale = 10;
		[Range(1, 30)] public int octaves = 6;
		[Range(0, 1)] public float persistence = 0.4f;
		[Range(1, 50)] public float lacunarity = 1;
		public int seed;
		public Vector2 offset;
		
	}

}