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
                    // TODO: Refactor this third loop so the complexity isn't O(w*h*k) where k is number of layers
                    for (int k = 0; k < tileSettings.layers.Length; k++) {
                        Layer layer = tileSettings.layers[k];
                        if (layer.minHeight <= heightMap.Values[i, j] && heightMap.Values[i, j] <= layer.maxHeight) {
                            tilemap.SetTile(new Vector3Int(i, j, 0), layer.tile);
                            break;
                        }

                    }
                    
                }
            }
            Debug.Log("Generated");
            Debug.Log("MinHeight" + heightMap.MinValue);
            Debug.Log("MaxHeight" + heightMap.MaxValue);
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