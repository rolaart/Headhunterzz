namespace Characters {

	public static class ExperienceTable {
		/** Diablo 2 experiences per level, up to the 20th level
		 *  @link https://diablo.fandom.com/wiki/Character_Level
		 */
		public const int MaxLevels = 20;

		private static readonly int[] Requirement = new int[MaxLevels] {
			0, 500, 1_500, 3_750, 7_875, 14_175, 22_680, 32_886, 44_396, 57_715, 72_144, 90_180,
			112_725, 140_906, 176_132, 220_165, 275_207, 344_008, 430_010, 537_513
		};

		public static int GetExperienceRequired(int level) {
			return Requirement[level % Requirement.Length];
		}

		/** e.g. if we are at level 1 and we have 510 experience, we should level up */
		public static bool ShouldLevelUp(int level, int experience) {
			return Requirement[level % Requirement.Length] < experience;
		}

		/** Should be called for the UI */
		public static int ExperienceLeft(int level, int experience) {
			return Requirement[level % Requirement.Length] - experience;
		}

		public static int GetExperienceAfterLevelUp(int level, int experience)
		{
			return experience % Requirement[level % Requirement.Length];
		}
	}

}