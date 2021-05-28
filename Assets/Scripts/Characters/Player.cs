using System;
using Characters.Abilities;
using UnityEngine;

namespace Characters {

	public class Player : Character {
		private Rigidbody2D _rigidBody;

		private readonly Ability[] _abilities = new Ability[1];

		private void Awake() {
			_rigidBody = GetComponent<Rigidbody2D>();
			_abilities[0] = new AbilityDash(KeyCode.Q, 5.0f, _rigidBody);
		}


		// Update is called once per frame
		void FixedUpdate() {
			UpdateMovement();
			UpdateAbilities();
		}

		private void UpdateMovement() {
			Vector2 currentPos = _rigidBody.position;
			Vector2 inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
			Vector2 movement = inputVector * DefaultMoveSpeed;
			Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;
			_rigidBody.MovePosition(newPos);
		}

		private void UpdateAbilities() {
			foreach (var ability in _abilities) {
				ability.Update();
			}
		}
	}

}