using System;
using System.Collections;
using Common;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;
using Utils;
using Random = UnityEngine.Random;
using Pathfinding;

namespace World {

	public class Island : MonoBehaviour
	{
		public Tilemap tilemap;
		public Tilemap waterTilemap;
		public Tilemap colliderTilemap;
		public AstarPath astarPath;
		
		public TileBase collideTile;
		public TileBase waterTile;

		
		private const int Scale = 6;
		private const int IslandSize = IslandChunk.IslandChunkSize * Scale;
		
		private void Start() {
			GameManager.Instance.InitializePlayer();
			
			Restart();
			CreateIslandFromMap();
		}

		private void CreateIslandFromMap() {
			Map map = FindObjectOfType<Map>();
			
			Assert.IsNotNull(map);
			Assert.IsNotNull(map.tilemap);
			
			CopyScaledFromMap(map);
			CreateCollisionMap();
			CreateSpawnPosition();
			StartCoroutine(CreatePathfindingArea());
			CreatePathfindingArea();
			
			map.gameObject.SetActive(false);
		}

		private void CopyScaledFromMap(Map map)
		{
			Vector3Int pos = map.SelectedIslandChunk.Position;
			
			const float invScale = (float) IslandChunk.IslandChunkSize / IslandSize;

			
			Vector3Int lowerBound = map.SelectedIslandChunk.Min.ModulusNegative(IslandChunk.IslandChunkSize);
			Vector3Int upperBound = map.SelectedIslandChunk.Max.ModulusNegative(IslandChunk.IslandChunkSize);

			lowerBound *= Scale;
			upperBound *= Scale;
			
			for (int i = lowerBound.x; i < upperBound.x; i++) {
				for (int j = lowerBound.y; j < upperBound.y; j++) {
					Vector3Int next = new Vector3Int(i, j, 0);
					Vector3Int nearestNeighbour = Vector3Int.FloorToInt(new Vector3(i * invScale, j * invScale, 0));
					
					if (map.tilemap.HasTile(pos + nearestNeighbour)) {
						tilemap.SetTile(next, map.SelectedIslandChunk.Biome.GetRandomTile());
					}
				}
			}
			
			
		}
		
		private void CreateCollisionMap() {
			Vector3Int left = Vector3Int.left;
			Vector3Int right = Vector3Int.right;
			Vector3Int up = Vector3Int.up;
			Vector3Int down = Vector3Int.down;

			for (int i = 0; i <= IslandSize; i++) {
				for (int j = 0; j <= IslandSize; j++) {
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

		private IEnumerator CreatePathfindingArea()
		{
			yield return new WaitForSeconds(5);
			astarPath.Scan();
		}
		
		private void CreateSpawnPosition() {
			// find the first available tile to spawn on
			for (int x = 2; x < IslandSize; x++) {
				for (int y = 2; y < IslandSize; y++) {
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

		private void Restart()
		{
			waterTilemap.ClearAllTiles();
			tilemap.ClearAllTiles();
			colliderTilemap.ClearAllTiles();
			
			for (int i = 0; i < IslandSize; i++) {
				for (int j = 0; j < IslandSize; j++) {
					Vector3Int offset = new Vector3Int(i, j, 0);
					waterTilemap.SetTile(offset, waterTile);
				}
			}
		}

		private void DebugBounds(Map map, Vector3Int lowerBound, Vector3Int upperBound)
		{
			for (int i = lowerBound.x ; i < upperBound.x; i++)
			{
				Vector3Int next = new Vector3Int(i, 0, 0);
				Vector3Int next2 = new Vector3Int(0, i, 0);
				
				tilemap.SetTile(next, map.tileSettings.borderLayer.tile);
				tilemap.SetTile(next2, map.tileSettings.borderLayer.tile);
			}
			for (int i = lowerBound.y; i < upperBound.y; i++)
			{
				Vector3Int next = new Vector3Int(i, 0, 0);
				Vector3Int next2 = new Vector3Int(0, i, 0);
				
				tilemap.SetTile(next, map.tileSettings.borderLayer.tile);
				tilemap.SetTile(next2, map.tileSettings.borderLayer.tile);
			}
		}
	}

}