using System;
using Characters.Abilities;
using Combat;
using Settings;
using UnityEngine;
using UnityEngine.Events;
using Utils.Managers;

namespace Characters {

	public class Player : Character {
		[SerializeField] 
		private KeybindsSettings keybindsSettings;
		[SerializeField]
		private AttackDefinition baseAttack;
		private readonly Ability[] _abilities = new Ability[1];
		private float timeOfLastAttack = float.MinValue;

		public readonly Inventory.Inventory Inventory = new Inventory.Inventory();
		
		protected override void Awake() {
			base.Awake();

			_abilities[0] = new AbilityDash(keybindsSettings.dashKey, 5.0f, _rigidbody);
			stats.IsPlayer = true;
		}

		// Update is called once per frame
		void FixedUpdate() {
			UpdateMovement();
			UpdateAbilities();
			UpdateAttack();
		}

		private void UpdateAttack() {
			if (Input.GetKeyDown(keybindsSettings.attackKey)) {
				float timeSinceLastAttack = Time.time - timeOfLastAttack;
				bool canAttack = timeSinceLastAttack > baseAttack.Cooldown;

				if (!canAttack)
				{
					Debug.Log("Player failed attacking");
					return;
				}
				
				Debug.Log("Player attacked");
				timeOfLastAttack = Time.time;
				
				SoundManager.Instance.Play(SoundType.SoundWeaponAttack);
				foreach (Enemy enemy in FindObjectsOfType<Enemy>()) {
					((Weapon) baseAttack).ExecuteAttack(gameObject, enemy.gameObject);
				}
				
			}
		}
		private void UpdateMovement() {
			Vector2 currentPos = _rigidbody.position;
			Vector2 inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
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

		#region Events

		public void RegisterOnLevelUpListener(UnityAction listener)
		{
			stats.RegisterOnLevelUpListener(listener);
		}

		public void RegisterOnDamagedListener(UnityAction listener)
		{
			stats.RegisterOnDamagedListener(listener);
		}

		public void RegisterOnGainedHealthListener(UnityAction listener)
		{
			stats.RegisterOnGainedHealthListener(listener);
		}
		
		public void RegisterOnExperienceGainedListener(UnityAction listener)
		{
			stats.RegisterOnExperienceGainedListener(listener);
		}
		public void RegisterOnDeathListener(UnityAction listener)
		{
			onPlayerDeath.AddListener(listener);
		}
		
		public void RegisterOnMobKilledListener(UnityAction<int> listener)
		{
			onMobKilled.AddListener(listener);
		}

		#endregion
		
	}

}