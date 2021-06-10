using UnityEngine;
using World.WorldGeneration;

namespace World
{
    /** An Island can be defined by 2 points, represented by a 2D Bounding Box.
	 *	The first point is formed by taking in account the minimum X coordinate
	 *  and minimum Y coordinate. It forms the @param min
	 *  vector. The second point is formed by the maximum X and Y coordinates
	 * therefore it forms a rectangle.
	 */
    public enum IslandDifficulty
    {
        Easy,
        Medium,
        Hard
    }

    public class IslandChunk
    {
        public const int IslandChunkSize = 64;

        public Biome Biome;

        // aligned to chunk coordinates
        public readonly Vector3Int Position;

        public readonly string Name;

        // actual bounds
        public Vector3Int Min;

        public Vector3Int Max;
        
        public IslandDifficulty Difficulty;

        public IslandChunk(Vector3Int position, string name)
        {
            Position = position;
            Name = name;
        }

        public void SetBounds(Vector3Int min, Vector3Int max)
        {
            Min = min;
            Max = max;

            CalculateDifficulty();
        }

        private void CalculateDifficulty()
        {
            Vector3Int diff = Max - Min;

            int approximateArea = diff.x * diff.y;

            bool isEasy = approximateArea <= 20;
            if (isEasy)
            {
                Difficulty = IslandDifficulty.Easy;
                return;
            }

            bool isMedium = approximateArea <= 40;
            if (isMedium)
            {
                Difficulty = IslandDifficulty.Medium;
                return;
            }

            bool isHard = approximateArea <= 60;
            if (isHard)
            {
                Difficulty = IslandDifficulty.Hard;
                return;
            }
        }
    }
}