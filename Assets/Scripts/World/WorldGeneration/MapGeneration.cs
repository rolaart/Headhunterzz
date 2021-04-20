using System;
using System.Collections.Generic;
using Settings;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;

namespace World.WorldGeneration {

	public class MapGeneration : MonoBehaviour {
		public int mapWidth = 500;
		public int mapHeight;
		public Tilemap tilemap;
		public HeightMapSettings heightMapSettings;
		public TileSettings tileSettings;

		private static Random random = new Random();

		public void Generate() {
			HeightMap heightMap = HeightMapGeneration.Generate(mapWidth, mapHeight, heightMapSettings);
			tilemap.ClearAllTiles();

			// for (int i = 0; i < mapWidth; i++) {
			//     for (int j = 0; j < mapHeight; j++) {
			//         tilemap.SetTile(new Vector3Int(i, j, 0), tileSettings.GetLayerTile(heightMap.Values[i, j]));
			//     }
			// }

			Queue<Vector2> islandsBorders = new Queue<Vector2>(8);
			
			int islandWidth = random.Next(Island.MIN_WIDTH, Island.MAX_WIDTH);
			int islandHeight = random.Next(Island.MIN_HEIGHT, Island.MAX_HEIGHT);

			for (int x = 0; x < islandWidth; x++) {
				for (int y = 0; y < islandHeight; y++) {
					tilemap.SetTile(
						new Vector3Int(x, y, 0),
						tileSettings.GetLayerTile(heightMap.Values[x, y])
					);
				}
			}
			
			islandsBorders.Enqueue(new Vector2(0, islandHeight));
			islandsBorders.Enqueue(new Vector2(islandWidth, 0));

			while (islandsBorders.Count > 0) {
				Vector2 nextPos = islandsBorders.Dequeue();
				nextPos += new Vector2(random.Next(Island.MIN_PADDING, Island.MAX_PADDING), random.Next(Island.MIN_PADDING, Island.MAX_PADDING));
				
				islandWidth = random.Next(Island.MIN_WIDTH, Island.MAX_WIDTH);
				islandHeight = random.Next(Island.MIN_HEIGHT, Island.MAX_HEIGHT);
				
				for (int x = 0; x < islandWidth; x++) {
					for (int y = 0; y < islandHeight; y++) {
						int realPosX = (int) nextPos.x + x;
						int realPosY = (int) nextPos.y + y;
						tilemap.SetTile(
							new Vector3Int(realPosX, realPosY, 0),
							tileSettings.GetLayerTile(Noise.GenerateNoise(realPosX, realPosY, heightMapSettings.noiseSettings))
						);
					}
				}

				if (nextPos.x > mapWidth || nextPos.y > mapHeight) break;
				
				islandsBorders.Enqueue(new Vector2(nextPos.x + islandWidth, nextPos.y + islandHeight));
			}

			// for (int i = 0; i < mapWidth / Island.MAX_WIDTH; i++) {
			// 	for (int j = 0; j < mapHeight / Island.MAX_HEIGHT; j++) {
			// 		// int islandWidth = random.Next(Island.MIN_WIDTH, Island.MAX_WIDTH);
			// 		// int islandHeight = random.Next(Island.MIN_HEIGHT, Island.MAX_HEIGHT);
			//
			// 		for (int x = 0; x < islandWidth; x++) {
			// 			for (int y = 0; y < islandHeight; y++) {
			// 				int realPosX = i * Island.MAX_WIDTH + x;
			// 				int realPosY = j * Island.MAX_HEIGHT + y;
			// 				tilemap.SetTile(
			// 					new Vector3Int(realPosX, realPosY, 0),
			// 					tileSettings.GetLayerTile(heightMap.Values[realPosX, realPosY])
			// 				);
			// 			}
			// 		}
			// 	}
			// }

			// bool isIsland = false;
			// for (int i = 0; i < mapWidth; i++) {
			//     for (int j = 0; j < mapHeight; j++) {
			//         if (tilemap.GetTile(new Vector3Int(i, j, 0)) == tileSettings.layers[1].tile) {
			//             if (tilemap.GetTile(new Vector3Int(i + 1, j, 0)) == tileSettings.layers[0].tile ||
			//                 tilemap.GetTile(new Vector3Int(i - 1, j, 0)) == tileSettings.layers[0].tile ||
			//                 tilemap.GetTile(new Vector3Int(i, j + 1, 0)) == tileSettings.layers[0].tile ||
			//                 tilemap.GetTile(new Vector3Int(i, j - 1, 0)) == tileSettings.layers[0].tile) {
			//                 tilemap.SetTile(new Vector3Int(i, j, 0), tileSettings.layers[2].tile);
			//                 isIsland = true;
			//             }
			//         }
			//     }
			// }

			// for (int i = 0; i <= mapWidth / Island.MAX_WIDTH; i++) {
			//     for (int j = 0; j <= mapHeight / Island.MAX_HEIGHT; j++) {
			//         
			//         int islandWidth = random.Next(Island.MIN_WIDTH, Island.MAX_WIDTH);
			//         int islandHeight = random.Next(Island.MIN_HEIGHT, Island.MAX_HEIGHT);
			//         
			//         for (int x = 0; x < islandWidth; x++) {
			//             for (int y = 0; y < islandHeight; y++) {
			//                 try {
			//                     tilemap.SetTile(
			//                         new Vector3Int(i * Island.MAX_WIDTH + x, j * Island.MAX_HEIGHT + y, 0),
			//                         tileSettings.GetLayerTile(heightMap.Values[i * Island.MAX_WIDTH + x,
			//                             j * Island.MAX_HEIGHT + y])
			//                     );
			//                 }
			//                 catch (IndexOutOfRangeException) {
			//                     Debug.Log("X"+ i * Island.MAX_WIDTH + x);
			//                     Debug.Log("Y"+ j * Island.MAX_HEIGHT + y);
			//                 }
			//             }
			//         }
			//
			//     }
			// }
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
	}

}