using UnityEngine;

namespace World {

	/** An Island can be defined by 2 points, represented by a 2D Bounding Box.
	 *	The first point is formed by taking in account the minimum X coordinate
	 *  and minimum Y coordinate. It forms the @param min
	 *  vector. The second point is formed by the maximum X and Y coordinates
	 * therefore it forms a rectangle.
	 */
	
	public class IslandChunk {
		public static int IslandChunkSize = 64;
		public string name = "England";

		public IslandChunk(string name) {
			this.name = name;
		}
	}

}