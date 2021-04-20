using UnityEngine;

namespace World {

	/** An Island can be defined by 2 points, represented by a 2D Bounding Box.
	 *	The first point is formed by taking in account the minimum X coordinate
	 *  and minimum Y coordinate where a ground tile appears. It forms the @param min
	 *  vector. The second point is formed by the maximum X and Y coordinates where
	 *  a ground tile is and therefore it forms a rectangle around the Island.
	 */
	public class Island {
		private Vector2 min;
		private Vector2 max;
	}

}