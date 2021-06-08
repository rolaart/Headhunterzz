using Characters;
using Common;
using UnityEngine;

namespace Combat
{
    [CreateAssetMenu(fileName = "Weapon.asset", menuName = "Attack/Weapon")]
    public class Weapon : AttackDefinition
    {
        public void ExecuteAttack(GameObject attacker, GameObject defender)
        {
            if (defender == null)
                return;

            // Check if defender is in range of the attacker
            if (Vector3.Distance(attacker.transform.position, defender.transform.position) > Range)
                return;

            // Check if defender is in front of the player
            if (!attacker.IsFacingTarget(defender))
                return;

            // at this point the attack will connect
            var attackerStats = attacker.GetComponent<Character>().stats;
            var defenderStats = defender.GetComponent<Character>().stats;

            var attack = CreateAttack(attackerStats, defenderStats);

            var attackable = defender.GetComponent<IAttackable>();
            
            attackable.OnAttack(attacker, attack);
        }
    }
}