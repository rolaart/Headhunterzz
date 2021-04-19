using System;
using Settings;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace World.WorldGeneration
{
    public class MapGeneration : MonoBehaviour {
        public int mapWidth;
        public int mapHeight;
        public Tilemap tilemap;
        public HeightMapSettings heightMapSettings;
        public TileSettings tileSettings;
        
        public void Generate() {
            HeightMap heightMap = HeightMapGeneration.Generate(mapWidth, mapHeight, heightMapSettings);
            
            for (int i = 0; i < mapWidth; i++) {
                for (int j = 0; j < mapHeight; j++) {
                    tilemap.SetTile(new Vector3Int(i, j, 0), tileSettings.GetLayerTile(heightMap.Values[i, j]));
                }
            }
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