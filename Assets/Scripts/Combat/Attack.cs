using Characters;
using UnityEngine;

namespace Combat {

	public class Attack {
		public readonly int Damage;
		public readonly bool IsCritical;

		public Attack(int damage, bool critical) {
			Damage = damage;
			IsCritical = critical;
		}
	}

}