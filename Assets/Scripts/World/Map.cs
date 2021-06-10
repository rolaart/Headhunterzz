#define DEBUG

using System;
using System.Collections.Generic;
using Settings;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using Utils.Managers;
using World.WorldGeneration;


namespace World {

	[ExecuteInEditMode]
	public class Map : MonoBehaviour {
		public Camera mainCamera;
		private Transform _cameraTransform;
		private const int ExplorationDistance = 2;

		public Tilemap tilemap;
		public Tilemap waterTilemap;
		public IslandChunk SelectedIslandChunk;

		public HeightMapSettings heightMapSettings;
		public TileSettings tileSettings;
		public BiomeSettings biomeSettings;
		
		public GameObject selectedIslandHoverEffect;
		
		[SerializeField]
		public MapGeneration _mapGeneration;
		public Dictionary<Vector3Int, MapChunk> Chunks = new Dictionary<Vector3Int, MapChunk>();
		
		
		
		public void Start() {
			
			_cameraTransform = mainCamera.GetComponent<Transform>();
			if (Application.isPlaying) {
				tilemap.ClearAllTiles();
				_mapGeneration =
					new MapGeneration(tilemap, waterTilemap, heightMapSettings, tileSettings, biomeSettings);
				gameObject.DontDestroyOnLoad();
			}
		}

		public void Update() {
			if (Application.isPlaying) {
				ExploreNewCoordinates();
				Vector3Int mousePos = tilemap.WorldToCell(mainCamera.ScreenToWorldPoint(Input.mousePosition));
				if (Input.GetMouseButtonDown(0)) {
					if (IsIslandMouseHit(mousePos)) {
						Vector3Int mapChunkPos =
							Vector3Int.FloorToInt((Vector3) mousePos / (float) MapChunk.MapChunkSize);

						MapChunk mapChunk = Chunks[mapChunkPos];
						SelectedIslandChunk = mapChunk.GetIslandFromMouse(mousePos);
						UpdateIslandHover();
						UpdateIslandBoundsIfNeeded();
						UIManager.Instance.ShowMapGui(SelectedIslandChunk);
					}
					else {
						if (!EventSystem.current.IsPointerOverGameObject()) {
							selectedIslandHoverEffect.SetActive(false);
							UIManager.Instance.HideMapGui();
						}
					}
				}
			}
		}

		private void UpdateIslandHover() {
			selectedIslandHoverEffect.SetActive(true);
			selectedIslandHoverEffect.transform.position =
				tilemap.CellToWorld(SelectedIslandChunk.Position + new Vector3Int(IslandChunk.IslandChunkSize / 2, IslandChunk.IslandChunkSize / 2, 0));

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
				MapChunk toGenerate = new MapChunk(pos);
				_mapGeneration.GenerateMapChunk(toGenerate);
				Chunks.Add(pos, toGenerate);
			}
		}

		public bool IsIslandMouseHit(Vector3Int mousePos) {
			return tilemap.HasTile(mousePos);
		}

		public void UpdateIslandBoundsIfNeeded() {
			Vector3Int min = new Vector3Int(Int32.MaxValue, Int32.MaxValue, 0);
			Vector3Int max = new Vector3Int(Int32.MinValue, Int32.MinValue, 0);

			Vector3Int islandPos = SelectedIslandChunk.Position;

			if (SelectedIslandChunk.Min == Vector3Int.zero && SelectedIslandChunk.Max == Vector3Int.zero) {
				for (int i = 0; i < IslandChunk.IslandChunkSize; i++) {
					for (int j = 0; j < IslandChunk.IslandChunkSize; j++) {
						Vector3Int next = new Vector3Int(i, j, 0) + islandPos;
						if (tilemap.HasTile(next)) {
							min.x = Math.Min(min.x, next.x);
							min.y = Math.Min(min.y, next.y);
							max.x = Math.Max(max.x, next.x);
							max.y = Math.Max(max.y, next.y);
						}
					}
				}

				SelectedIslandChunk.SetBounds(min, max);
			}
		}

		public void TeleportToSelectedIsland()
		{
			GameManager.Instance.OnIslandVisit(SelectedIslandChunk);
		}

#if UNITY_EDITOR
		public void Generate() {
			waterTilemap.ClearAllTiles();
			tilemap.ClearAllTiles();
			_mapGeneration = new MapGeneration(tilemap, waterTilemap, heightMapSettings, tileSettings, biomeSettings);
			
			Vector3Int pos = new Vector3Int(0, 0, 0);
			MapChunk toGenerate = new MapChunk(pos);
			_mapGeneration.GenerateMapChunk(toGenerate);
			Chunks.Add(pos, toGenerate);
		}

		void OnValuesUpdated() {
			if (!Application.isPlaying) {
				Generate();
			}
		}

		void OnValidate() {
			if (tileSettings != null) {
				tileSettings.OnValuesUpdated -= OnValuesUpdated;
				tileSettings.OnValuesUpdated += OnValuesUpdated;
			}

			if (heightMapSettings != null) {
				heightMapSettings.OnValuesUpdated -= OnValuesUpdated;
				heightMapSettings.OnValuesUpdated += OnValuesUpdated;
			}
		}
#endif
	}

}