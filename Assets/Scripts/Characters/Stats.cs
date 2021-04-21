using System;

namespace Characters {

	public enum StatsType : byte {
		// Core Stats
		Strength = 0,
		Stamina = 1,
		Charisma = 2,

		// Secondary Stats
		Lifesteal = 3,

		// Leveling Stats
		Experience = 4,
		Level = 5
	}

	public class Stats {
		private int _availablePoints = 0;
		private const int PointsPerLevel = 5;

		/** Points in the stat */
		public readonly int[] Points = new int[Enum.GetNames(typeof(StatsType)).Length];

		public int Strength {
			get => Points[(int) StatsType.Strength];
			set => Points[(int) StatsType.Strength] = value;
		}

		public int Stamina {
			get => Points[(int) StatsType.Stamina];
			set => Points[(int) StatsType.Stamina] = value;
		}

		public int Charisma {
			get => Points[(int) StatsType.Charisma];
			set => Points[(int) StatsType.Charisma] = value;
		}

		public int Experience {
			get => Points[(int) StatsType.Experience];
			// Up to Max level's experience
			set => Points[(int) StatsType.Experience] = value % ExperienceTable.GetExperienceRequired(ExperienceTable.MaxLevels);
		}

		public int Level {
			get => Points[(int) StatsType.Level];
			// Up to Max Level
			set => Points[(int) StatsType.Level] = value % ExperienceTable.MaxLevels;
		}

		public int Lifesteal {
			get => Points[(int) StatsType.Lifesteal];
			// 100% Max
			set => Points[(int) StatsType.Lifesteal] = value % 100;
		}

		// TODO Play with the coefficients scaling 
		public int Damage => Strength * 10;
		public int Health => Stamina * 10;
		public float AdditionalGold => Charisma * 0.2f;
		/** Percent of damage done, regained as health */
		public float HealthRegainFromAttack => Damage * (Lifesteal / 100.0f);

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