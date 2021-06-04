using Characters;
using UnityEngine;

namespace Combat {
	[CreateAssetMenu(fileName = "Attack.asset", menuName = "Attack/BaseAttack")]
	public class AttackDefinition : ScriptableObject{
		public float Cooldown;
		public float Range;
		public float MinDamage;
		public float MaxDamage;
		
		public Attack CreateAttack(CharacterStats attackerStats, CharacterStats defenderStats) {
			float coreDamage = attackerStats.Damage;
			coreDamage += Random.Range(MinDamage, MaxDamage);

			bool isCritical = Random.value < attackerStats.CriticalChance;
			if (isCritical) {
				coreDamage *= 2;
			}

			return new Attack((int) coreDamage, isCritical);
		}
	}

}