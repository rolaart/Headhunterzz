using System;
using Characters.Abilities;
using Combat;
using Settings;
using UnityEngine;

namespace Characters {

	public class Player : Character {
		[SerializeField] 
		private KeybindsSettings keybindsSettings;
		[SerializeField]
		private AttackDefinition baseAttack;
		private readonly Ability[] _abilities = new Ability[1];

		private void Awake() {
			_abilities[0] = new AbilityDash(keybindsSettings.dashKey, 5.0f, _rigidbody);
		}
		
		// Update is called once per frame
		void FixedUpdate() {
			UpdateMovement();
			UpdateAbilities();
			UpdateAttack();
		}

		private void UpdateAttack() {
			if (Input.GetKeyDown(keybindsSettings.attackKey)) {
				Debug.Log("Attack Key Pressed");
				foreach (Enemy enemy in FindObjectsOfType<Enemy>()) {
					Debug.Log("Checking enemy");
					var attackable = enemy.GetComponent<IAttackable>();
					var attack = baseAttack.CreateAttack(stats, enemy.stats);
					
					attackable.OnAttack(gameObject, attack);
				}
				
			}
		}
		private void UpdateMovement() {
			Vector2 currentPos = _rigidbody.position;
			Vector2 inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
			Vector2 movement = inputVector * DefaultMoveSpeed;
			Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;
			SetDirection(movement);
			_rigidbody.MovePosition(newPos);
		}

		private void UpdateAbilities() {
			foreach (var ability in _abilities) {
				ability.Update();
			}
		}
	}

}