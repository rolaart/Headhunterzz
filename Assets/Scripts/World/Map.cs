using System;
using System.Collections.Generic;
using Settings;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using World.WorldGeneration;


namespace World {

	public class Map : MonoBehaviour {
		public Camera camera;
		private Transform _cameraTransform;
		private const int ExplorationDistance = 2;
		
		public Tilemap tilemap;
		public IslandChunk SelectedIslandChunk;
		
		public HeightMapSettings heightMapSettings;
		public TileSettings tileSettings;
		public BiomeSettings biomeSettings;
		
		private MapGeneration _mapGeneration;
		public Dictionary<Vector3Int, MapChunk> Chunks = new Dictionary<Vector3Int, MapChunk>();


		public void Start() {
			_mapGeneration = new MapGeneration(tilemap, heightMapSettings, tileSettings, biomeSettings);
			_cameraTransform = camera.GetComponent<Transform>();
			
			gameObject.DontDestroyOnLoad();
		}

		public void Update() {
			ExploreNewCoordinates();
			if (Input.GetMouseButtonDown(0)) {
				Vector3Int mousePos = tilemap.WorldToCell(camera.ScreenToWorldPoint(Input.mousePosition));
				
				Debug.Log(mousePos);
				if (IsIslandMouseHit(mousePos)) {
					// tilemap.SetTile(mousePos, tileSettings.layers[2].tile);
					Debug.Log("Island is hit");
					Vector3Int mapChunkPos = mousePos / MapChunk.MapChunkSize;
					Vector3Int islandChunkPos = (mousePos / IslandChunk.IslandChunkSize);
					islandChunkPos.x %= MapChunk.RowSize;
					islandChunkPos.y %= MapChunk.RowSize;

					MapChunk mapChunk = GetMapChunk(mapChunkPos);
					SelectedIslandChunk = mapChunk.GetIsland(islandChunkPos);
					Debug.Log("Island is " + SelectedIslandChunk.Name);
					TeleportToIsland();
				}
				else {
					Debug.Log("Island is missed");
				}
			}
		}

		public void OnDestroy() {
			DontDestroyOnLoadManager.DestroyAll();
		}

		private void ExploreNewCoordinates() {
			Vector3Int cameraTilePos = tilemap.WorldToCell(camera.ScreenToWorldPoint(_cameraTransform.position));
			Vector3Int cameraMapChunkPos = cameraTilePos / MapChunk.MapChunkSize;
			for (int i = cameraMapChunkPos.x - ExplorationDistance; i < cameraMapChunkPos.x + ExplorationDistance; i++) {
				for (int j = cameraMapChunkPos.y - ExplorationDistance; j < cameraMapChunkPos.y + ExplorationDistance; j++) {
					GetMapChunk(new Vector3Int(i, j, 0));
				}
			}
		}

		MapChunk GetMapChunk(Vector3Int pos) {
			if (!Chunks.ContainsKey(pos)) {
				_mapGeneration.GenerateMapChunk(pos);
				Chunks.Add(pos, new MapChunk(pos));
			}

			return Chunks[pos];
		}

		private void TeleportToIsland() {
			SceneManager.LoadScene("IslandScene");
		}

		public bool IsIslandMouseHit(Vector3Int mousePos) {
			return tilemap.GetTile(mousePos) == tileSettings.groundLayer.tile;
		}
	}

}