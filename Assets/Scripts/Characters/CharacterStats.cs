using System;
using UnityEngine;

namespace Characters {

	[Serializable]
	public class NamedArrayAttribute : PropertyAttribute {
		public readonly string[] names;

		public NamedArrayAttribute(string[] names) {
			this.names = names;
		}
	}

	[CreateAssetMenu(fileName = "NewStats", menuName = "Character/Stats", order = 1)]
	public class CharacterStats : ScriptableObject {
		private int _availablePoints = 0;
		private const int PointsPerLevel = 5;


		public int Strength;

		public int Stamina;

		public int Luck;

		public int Charisma;

		public int Experience;

		public int Level;

		public int Lifesteal;

		// TODO Play with the coefficients scaling 
		public int Damage => Strength * 10;
		public int Health => Stamina * 10;
		public float AdditionalGold => Charisma * 0.2f;
		public float CriticalChance => Luck * 0.01f;

		/** Percent of damage done, regained as health */
		public float RegainFromAttack => Lifesteal * 0.01f;

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
	}

}