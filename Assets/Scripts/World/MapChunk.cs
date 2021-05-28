using System.Collections.Generic;
using UnityEngine;

namespace World {
	/// <summary>
	/// A MapChunk consists of smaller island chunks, which represent the area,
	/// where a given island can generate
	/// </summary>
	public class MapChunk {
		public static int MapChunkSize = 256;
		public static int RowSize = MapChunkSize / IslandChunk.IslandChunkSize;

		public IslandChunk[,] Chunks = new IslandChunk[RowSize, RowSize];

		public MapChunk() {
			for (int i = 0; i < RowSize; i++) {
				for (int j = 0; j < RowSize; j++) {
					Chunks[i, j] = new IslandChunk(IslandNamesDB.GetRandomName());
				}
			}
		}
		public IslandChunk GetIsland(Vector3Int pos) {
			return Chunks[pos.x, pos.y];
		}
	}

}