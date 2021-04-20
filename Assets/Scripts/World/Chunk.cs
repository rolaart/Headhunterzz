using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace World {
	public class Chunk {
		public static int CHUNK_SIZE_X = 32;
		public static int CHUNK_SIZE_Z = 32;

		public Vector2 min;
		public Vector2 max;
	}

}