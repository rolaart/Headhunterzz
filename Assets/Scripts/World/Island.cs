using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

namespace World {

	public class Island : MonoBehaviour {
		public Camera camera;
		public Tilemap tilemap;
		public Tilemap waterTilemap;
		private Vector3Int spawnPos;

		private void Start() {
			CreateIslandFromMap();

			camera.GetComponent<Transform>().localPosition = tilemap.CellToWorld(spawnPos);
			camera.GetComponent<Transform>().Translate(0, 0, -1);
		}

		private void CreateIslandFromMap() {
			Map map = FindObjectOfType<Map>();
			Assert.IsNotNull(map);
			Assert.IsNotNull(map.tilemap);
			Vector3Int pos = map.SelectedIslandChunk.Position;
			
			for (int i = 0; i < IslandChunk.IslandChunkSize; i++) {
				for (int j = 0; j < IslandChunk.IslandChunkSize; j++) {
					Vector3Int offset = new Vector3Int(i, j, 0);
					TileBase tile = map.waterTilemap.GetTile(pos + offset);
					waterTilemap.SetTile(offset, tile);
				}
			}
			
			for (int i = 0; i < IslandChunk.IslandChunkSize; i++) {
				for (int j = 0; j < IslandChunk.IslandChunkSize; j++) {
					Vector3Int offset = new Vector3Int(i, j, 0);
					TileBase tile = map.tilemap.GetTile(pos + offset);
					tilemap.SetTile(offset, tile);
				}
			}
			
			CreateSpawnPosition(map);
			
			map.gameObject.SetActive(false);
			
		}

		private void CreateSpawnPosition(Map map) {
			// find the first available tile to spawn on
			
			for (int i = 0; i < IslandChunk.IslandChunkSize; i++) {
				for (int j = 0; j < IslandChunk.IslandChunkSize; j++) {
					Vector3Int pos = new Vector3Int(i, j, 0);
					TileBase tile = tilemap.GetTile(pos);
					if (tile != null) {
						spawnPos = pos;
						return;
					}
				}
			}
		}
	}

}