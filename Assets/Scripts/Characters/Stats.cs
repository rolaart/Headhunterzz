using System;

namespace Characters {

	public enum StatsType {
		// Core Stats
		Strength = 0,
		Stamina = 1,
		Charisma = 2,
		
		// Leveling Stats
		Experience = 3,
		Level = 4
	}
	
	public class Stats {
		private int _availablePoints = 0;
		private const int PointsPerLevel = 5;
		/** Points in the stat */
		public readonly int[] Points = new int[Enum.GetNames(typeof(StatsType)).Length];

		public int Strength {
			get => Points[0];
			set => Points[0] = value;
		}

		public int Stamina {
			get => Points[1];
			set => Points[1] = value;
		}

		public int Charisma {
			get => Points[2];
			set => Points[2] = value;
		}

		public int Experience {
			get => Points[3];
			set => Points[3] = value;
		}

		public int Level {
			get => Points[4];
			set => Points[4] = value;
		}

		// TODO Play with the coefficients scaling 
		public int Damage => Strength * 10;
		public int Health => Stamina * 10;
		public float AdditionalGold => Charisma * 0.2f;

		public int GetPointsInStat(StatsType statsType) {
			return Points[(int) statsType];
		}

		private void OnLevelUp() {
			// leveling up
			Level++;
			// adding points to spend
			_availablePoints += PointsPerLevel;
			// resetting the experience
			Experience = 0;
		}

		public void OnMobKilled(int experience) {
			Experience += experience;

			if (ExperienceTable.ShouldLevelUp(Level, Experience)) {
				OnLevelUp();
			}
		}

		/** Should be called for the UI */
		public int GetAvailablePoints() {
			return _availablePoints;
		}

		/** Should be called from the UI */
		public void OnPointSpend(StatsType statsType) {
			Points[(int) statsType]++;
			_availablePoints--;
		}
	}

}