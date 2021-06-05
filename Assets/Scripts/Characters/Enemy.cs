using System;
using Combat;
using Pathfinding;
using UnityEngine;

namespace Characters {

	public class Enemy : Character, IAttackable
	{
		private AIPath _aiPath;

		private void Awake()
		{
			_aiPath = GetComponent<AIPath>();
		}

		void Update()
		{
			
			SetDirection(_aiPath.velocity);
		}

		public void OnAttack(GameObject attacker, Attack attack) {
			if (attack.IsCritical)
				Debug.Log("CRITICAL DAMAGE !!");

			Debug.LogFormat("{0} attacked {1} for {2} damage.", attacker.name, name, attack.Damage);
		}
	}

}