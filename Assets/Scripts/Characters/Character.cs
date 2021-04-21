using System;
using UnityEngine;

namespace Characters {

	[RequireComponent(typeof(Sprite),
		typeof(BoxCollider2D),
		typeof(Animator))]
	public class Character : MonoBehaviour {
		public const float DefaultMoveSpeed = 5.0f;
		public const float DefaultAttackSpeed = 2.0f;
		
		private Animator _animator;
		private Stats _stats;

		protected virtual void Start() {
			_animator = GetComponent<Animator>();
			_stats = new Stats();
		}
	}

}