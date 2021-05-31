using System;
using System.Collections.Generic;
using Settings;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace World.WorldGeneration {

	[Serializable]

	public class MapGeneration {

		private readonly Tilemap tilemap;
		private readonly Tilemap waterTilemap;
		private readonly HeightMapSettings heightMapSettings;
		private readonly TileSettings tileSettings;
		private readonly BiomeSettings biomeSettings;

		public MapGeneration(Tilemap tilemap, Tilemap waterTilemap, HeightMapSettings heightMapSettings,
			TileSettings tileSettings, BiomeSettings biomeSettings) {
			this.tilemap = tilemap;
			this.waterTilemap = waterTilemap;
			this.heightMapSettings = heightMapSettings;
			this.tileSettings = tileSettings;
			this.biomeSettings = biomeSettings;
		}

		Vector3Int GenerateRandomCenter(Vector3Int islandPos) {
			const int BORDER_PADDING = 10;
			int centerX = Random.Range(islandPos.x + BORDER_PADDING, islandPos.x + IslandChunk.IslandChunkSize - BORDER_PADDING);
			int centerY = Random.Range(islandPos.y + BORDER_PADDING, islandPos.y + IslandChunk.IslandChunkSize - BORDER_PADDING);
			return new Vector3Int(centerX, centerY, 0);
		}

		float CalculateAdjustedHeight(Vector3Int center, Vector3Int pos, float heightMapCellValue) {
			const float distanceWeight = 4.0f;
			const float heightMapWeight = 3.5f;
			// the more is the distance from the center to the current cell, 
			// the less it is possible to have a ground
			float distance = Vector3Int.Distance(center, pos);
			if (distance > 16.0f) return 0;
			float invDistance = distanceWeight / distance;
			return heightMapCellValue * heightMapWeight * invDistance;
		}

		void GenerateIslandChunkBorders(int chunkX, int chunkY) {
			for (int x = 0; x < IslandChunk.IslandChunkSize; x++) {
				tilemap.SetTile(new Vector3Int(chunkX + x, chunkY, 0), tileSettings.borderLayer.tile);
				tilemap.SetTile(new Vector3Int(chunkX + x, chunkY + IslandChunk.IslandChunkSize, 0),
					tileSettings.borderLayer.tile);
			}

			for (int y = 0; y < IslandChunk.IslandChunkSize; y++) {
				tilemap.SetTile(new Vector3Int(chunkX, chunkY + y, 0), tileSettings.borderLayer.tile);
				tilemap.SetTile(new Vector3Int(chunkX + IslandChunk.IslandChunkSize, chunkY + y, 0),
					tileSettings.borderLayer.tile);
			}
		}

		void GenerateIslandChunk(IslandChunk islandChunk, Vector3Int centroid) {
			const int BorderPadding = 2;
			HeightMap heightMap = HeightMapGeneration.Generate(islandChunk.Position.x, islandChunk.Position.y, IslandChunk.IslandChunkSize,
				IslandChunk.IslandChunkSize, heightMapSettings);
			islandChunk.Biome = BiomeGeneration.Generate(islandChunk.Position.x, islandChunk.Position.y, biomeSettings);

			for (int i = 0; i < BorderPadding; i++) {
				for (int j = 0; j < IslandChunk.IslandChunkSize; j++) {
					Vector3Int pos = new Vector3Int(i, j, 0) + islandChunk.Position;
					Vector3Int pos2 = new Vector3Int(j, i, 0) + islandChunk.Position;
					waterTilemap.SetTile(pos, tileSettings.waterLayer.tile);
					waterTilemap.SetTile(pos2, tileSettings.waterLayer.tile);
				}
			}

			for (int i = IslandChunk.IslandChunkSize - BorderPadding; i < IslandChunk.IslandChunkSize; i++) {
				for (int j = 0; j < IslandChunk.IslandChunkSize; j++) {
					Vector3Int pos = new Vector3Int(i, j, 0) + islandChunk.Position;
					Vector3Int pos2 = new Vector3Int(j, i, 0) + islandChunk.Position;
					waterTilemap.SetTile(pos, tileSettings.waterLayer.tile);
					waterTilemap.SetTile(pos2, tileSettings.waterLayer.tile);
				}
			}

			for (int x = BorderPadding; x < IslandChunk.IslandChunkSize - BorderPadding; x++) {
				for (int y = BorderPadding; y < IslandChunk.IslandChunkSize - BorderPadding; y++) {
					Vector3Int pos = new Vector3Int(x, y, 0) + islandChunk.Position;

					float height = CalculateAdjustedHeight(centroid, pos, heightMap.Values[x, y]);
					height = Mathf.Clamp01(height);
					if (height <= tileSettings.waterLayer.maxHeight) {
						waterTilemap.SetTile(pos, tileSettings.waterLayer.tile);
						continue;
					}

					if (height <= tileSettings.groundLayer.maxHeight) {
						tilemap.SetTile(pos, islandChunk.Biome.baseTile);
						continue;
					}

					if (height <= tileSettings.borderLayer.maxHeight) {
						tilemap.SetTile(pos, tileSettings.borderLayer.tile);
						continue;
					}
				}
			}
		}

		public void GenerateMapChunk(MapChunk mapChunk) {
			for (int i = 0; i < MapChunk.RowSize; i++) {
				for (int j = 0; j < MapChunk.RowSize; j++) {
					IslandChunk islandChunk = mapChunk.GetIslandFromIndex(new Vector3Int(i, j, 0));
					Vector3Int center = GenerateRandomCenter(islandChunk.Position);

					GenerateIslandChunk(islandChunk, center);
					//GenerateIslandChunkBorders(realIslandX, realIslandY);
				}
			}
		}
	}

}