using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace World {
	/// <summary>
	/// A MapChunk consists of smaller island chunks, which represent the area,
	/// where a given island can generate
	/// </summary>
	public class MapChunk {
		public const int MapChunkSize = 256;
		public const int RowSize = MapChunkSize / IslandChunk.IslandChunkSize;
		public Vector3Int Position;

		private readonly IslandChunk[][] _chunks = new IslandChunk[RowSize][];

		public MapChunk(Vector3Int position) {
			Position = position * MapChunkSize;
			
			for (int i = 0; i < RowSize; i++) {
				_chunks[i] = new IslandChunk[RowSize];
				for (int j = 0; j < RowSize; j++) {
					Vector3Int offset = new Vector3Int(i * IslandChunk.IslandChunkSize, j * IslandChunk.IslandChunkSize,
						0);
					Vector3Int islandPos = Position + offset;
					_chunks[i][j] = new IslandChunk(islandPos, IslandNamesDB.GetRandomName());
				}
			}
		}

		public IslandChunk GetIsland(Vector3Int mousePos) {
			Vector3Int islandIdx = mousePos;
			islandIdx.x = Math.Abs(islandIdx.x) % RowSize;
			islandIdx.y = Math.Abs(islandIdx.y) % RowSize;
			return _chunks[islandIdx.x][islandIdx.y];
		}
	}

}