using UnityEngine;

namespace World {

	/** An Island can be defined by 2 points, represented by a 2D Bounding Box.
	 *	The first point is formed by taking in account the minimum X coordinate
	 *  and minimum Y coordinate. It forms the @param min
	 *  vector. The second point is formed by the maximum X and Y coordinates
	 * therefore it forms a rectangle.
	 */
	
	public class IslandChunk {
		public const int IslandChunkSize = 64;
		// aligned to chunk coordinates
		public readonly Vector3Int Position;
		public readonly string Name;
		// actual bounds
		public Vector3Int Min;
		public Vector3Int Max;

		public IslandChunk(Vector3Int position, string name) {
			Position = position;
			Name = name;
		}
	}

}