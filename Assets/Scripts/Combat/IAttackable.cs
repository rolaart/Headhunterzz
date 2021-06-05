using UnityEngine;

namespace Combat {

	public interface IAttackable {
		public void OnAttack(GameObject attacker, Attack attack);
	}

}