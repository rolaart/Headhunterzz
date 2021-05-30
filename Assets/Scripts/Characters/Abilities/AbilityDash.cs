using UnityEngine;

namespace Characters.Abilities {

	public class AbilityDash : Ability {
		private readonly Rigidbody2D _rigidBody;
		private const int Thrust = 70;

		public AbilityDash(KeyCode keyBind, float cooldown, Rigidbody2D body) : base(keyBind, cooldown) {
			_rigidBody = body;
		}

		public override bool IsUsable() {
			return base.IsUsable() && (Input.GetAxis("Horizontal") != 0.0f || Input.GetAxis("Vertical") != 0.0f);
		}

		public override void Use() {
			Vector2 currentPos = _rigidBody.position;
			Vector2 inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
			Vector2 movement = inputVector * Thrust;
			Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;
			_rigidBody.MovePosition(newPos);
		}
	}

}