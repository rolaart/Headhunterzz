using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;
using Utils;
using Random = UnityEngine.Random;

namespace World {

	public class Island : MonoBehaviour
	{
		public Tilemap tilemap;
		public Tilemap waterTilemap;
		public Tilemap colliderTilemap;

		public TileBase collideTile;

		private void Start() {
			GameManager.Instance.InitializePlayer();
			
			CreateIslandFromMap();
		}

		private void CreateIslandFromMap() {
			Map map = FindObjectOfType<Map>();
			Assert.IsNotNull(map);
			Assert.IsNotNull(map.tilemap);
			Vector3Int pos = map.SelectedIslandChunk.Position;

			tilemap.ClearAllTiles();
			TileBase waterTile = map.waterTilemap.GetTile(Vector3Int.zero);
			for (int i = -IslandChunk.IslandChunkSize; i < IslandChunk.IslandChunkSize * 2; i++) {
				for (int j = -IslandChunk.IslandChunkSize; j < IslandChunk.IslandChunkSize * 2; j++) {
					Vector3Int offset = new Vector3Int(i, j, 0);

					waterTilemap.SetTile(offset, waterTile);
				}
			}

			for (int i = 0; i < IslandChunk.IslandChunkSize; i++) {
				for (int j = 0; j < IslandChunk.IslandChunkSize; j++) {
					Vector3Int offset = new Vector3Int(i, j, 0);
					if (map.tilemap.HasTile(pos + offset)) {
						tilemap.SetTile(offset, map.SelectedIslandChunk.Biome.GetRandomTile());
					}
				}
			}

			CreateCollisionMap(map);
			CreateSpawnPosition(map);

			map.gameObject.SetActive(false);
		}

		private void CreateCollisionMap(Map map) {
			Vector3Int left = Vector3Int.left;
			Vector3Int right = Vector3Int.right;
			Vector3Int up = Vector3Int.up;
			Vector3Int down = Vector3Int.down;

			for (int i = 0; i <= IslandChunk.IslandChunkSize; i++) {
				for (int j = 0; j <= IslandChunk.IslandChunkSize; j++) {
					Vector3Int pos = new Vector3Int(i, j, 0);
					if (tilemap.HasTile(pos)) {
						if (!tilemap.HasTile(pos + left)) colliderTilemap.SetTile(pos + left, collideTile);
						if (!tilemap.HasTile(pos + right)) colliderTilemap.SetTile(pos + right, collideTile);
						if (!tilemap.HasTile(pos + up)) colliderTilemap.SetTile(pos + up, collideTile);
						if (!tilemap.HasTile(pos + down)) colliderTilemap.SetTile(pos + down, collideTile);
					}
				}
			}
		}

		private void CreateSpawnPosition(Map map) {
			// find the first available tile to spawn on
			for (int x = 2; x < IslandChunk.IslandChunkSize; x++) {
				for (int y = 2; y < IslandChunk.IslandChunkSize; y++) {
					Vector3Int spawnPos = new Vector3Int(x, y, 0);
					if (tilemap.HasTile(spawnPos)) {
						// FIXME
						Vector3 position = tilemap.CellToWorld(spawnPos + new Vector3Int(4, 4, 0));
						GameManager.Instance.player.transform.position = position;
						GameManager.Instance.OnPlayerRespawned();
						return;
					}
				}
			}
		}
	}

}