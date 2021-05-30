#define DEBUG

using System;
using System.Collections.Generic;
using Settings;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using World.WorldGeneration;


namespace World {

	[Serializable]
	public struct MapGUI {
		[SerializeField] public GameObject panel;
		
		[SerializeField] public TextMeshProUGUI islandName;

		[SerializeField] public TextMeshProUGUI islandMin;
		
#if DEBUG
		[SerializeField] public TextMeshProUGUI islandMax;

		[SerializeField] public TextMeshProUGUI islandChunk;

		[SerializeField] public TextMeshProUGUI mapChunk;
#endif
	}

	public class Map : MonoBehaviour {
		[FormerlySerializedAs("camera")] public Camera mainCamera;
		private Transform _cameraTransform;
		private const int ExplorationDistance = 2;

		public Tilemap tilemap;
		public Tilemap waterTilemap;
		public IslandChunk SelectedIslandChunk;

		public HeightMapSettings heightMapSettings;
		public TileSettings tileSettings;
		public BiomeSettings biomeSettings;

		private MapGeneration _mapGeneration;
		public Dictionary<Vector3Int, MapChunk> Chunks = new Dictionary<Vector3Int, MapChunk>();

		public MapGUI mapGui;

		public TextMeshProUGUI mousePosDebug;


		public void Start() {
			_mapGeneration = new MapGeneration(tilemap, waterTilemap, heightMapSettings, tileSettings, biomeSettings);
			_cameraTransform = mainCamera.GetComponent<Transform>();

			gameObject.DontDestroyOnLoad();
		}

		public void Update() {
			ExploreNewCoordinates();
			Vector3Int mousePos = tilemap.WorldToCell(mainCamera.ScreenToWorldPoint(Input.mousePosition));
			mousePosDebug.text = ((Vector2Int) (mousePos)).ToString();
			if (Input.GetMouseButtonDown(0)) {
				if (IsIslandMouseHit(mousePos)) {
					Vector3Int mapChunkPos = Vector3Int.FloorToInt((Vector3) mousePos / (float) MapChunk.MapChunkSize);

					MapChunk mapChunk = Chunks[mapChunkPos];
					SelectedIslandChunk = mapChunk.GetIsland(mousePos);
#if DEBUG
					mapGui.mapChunk.text = ((Vector2Int) (mapChunkPos)).ToString();
#endif
					UpdateIslandBoundsIfNeeded(SelectedIslandChunk);
					UpdateGUI();
				}
				else {
					mapGui.panel.SetActive(false);
				}
			}
		}

		public void OnDestroy() {
			DontDestroyOnLoadManager.DestroyAll();
		}

		private void ExploreNewCoordinates() {
			Vector3Int cameraTilePos = tilemap.WorldToCell(mainCamera.ScreenToWorldPoint(_cameraTransform.position));
			Vector3Int cameraMapChunkPos = Vector3Int.FloorToInt(cameraTilePos / MapChunk.MapChunkSize);
			for (int i = cameraMapChunkPos.x - ExplorationDistance;
				i < cameraMapChunkPos.x + ExplorationDistance;
				i++) {
				for (int j = cameraMapChunkPos.y - ExplorationDistance;
					j < cameraMapChunkPos.y + ExplorationDistance;
					j++) {
					GenerateChunk(new Vector3Int(i, j, 0));
				}
			}
		}

		void GenerateChunk(Vector3Int pos) {
			if (!Chunks.ContainsKey(pos)) {
				_mapGeneration.GenerateMapChunk(pos);
				Chunks.Add(pos, new MapChunk(pos));
			}
		}

		public void TeleportToIsland() {
			SceneManager.LoadScene("IslandScene");
		}

		public bool IsIslandMouseHit(Vector3Int mousePos) {
			return tilemap.GetTile(mousePos) != tileSettings.waterLayer.tile;
		}

		public void UpdateIslandBoundsIfNeeded(IslandChunk islandChunk) {
			Vector3Int min = new Vector3Int(Int32.MaxValue, Int32.MaxValue, 0);
			Vector3Int max = new Vector3Int(Int32.MinValue, Int32.MinValue, 0);

			Vector3Int islandPos = islandChunk.Position;

			if (islandChunk.Min == Vector3Int.zero && islandChunk.Max == Vector3Int.zero) {
				for (int i = 0; i < IslandChunk.IslandChunkSize; i++) {
					for (int j = 0; j < IslandChunk.IslandChunkSize; j++) {
						Vector3Int next = new Vector3Int(i, j, 0) + islandPos;
						if (tilemap.GetTile(next) != tileSettings.waterLayer.tile) {
							min.x = Math.Min(min.x, next.x);
							min.y = Math.Min(min.y, next.y);
							max.x = Math.Max(max.x, next.x);
							max.y = Math.Max(max.y, next.y);
						}
					}
				}

				islandChunk.Min = min;
				islandChunk.Max = max;
			}
		}

		private void UpdateGUI() {
			mapGui.panel.SetActive(true);
			mapGui.islandName.text = SelectedIslandChunk.Name;
			mapGui.islandMin.text = ((Vector2Int) (SelectedIslandChunk.Min)).ToString();
#if DEBUG
			mapGui.islandMax.text = ((Vector2Int) (SelectedIslandChunk.Max)).ToString();
			mapGui.islandChunk.text = ((Vector2Int) (SelectedIslandChunk.Position)).ToString();
#endif
		}
	}

}