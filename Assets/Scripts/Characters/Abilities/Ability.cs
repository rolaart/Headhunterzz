using UnityEngine;

namespace Characters.Abilities {

	public abstract class Ability {
		private readonly KeyCode KeyBind;
		private readonly float Cooldown;
		private float CurrentTime;

		public Ability(KeyCode keyBind, float cooldown) {
			KeyBind = keyBind;
			Cooldown = cooldown;
			CurrentTime = cooldown;
		}

		public virtual bool IsUsable() {
			return CurrentTime > Cooldown;
		}

		public bool IsUsed() {
			return Input.GetKeyDown(KeyBind);
		}

		public void Update() {
			CurrentTime += Time.fixedDeltaTime;
			if (!IsUsed()) return;
			if (!IsUsable()) return;
			
			Use();
			Debug.Log("Ability Used.");
			Reset();
		}

		public void Reset() {
			CurrentTime = 0.0f;
		}

		public abstract void Use();
	
	}

}