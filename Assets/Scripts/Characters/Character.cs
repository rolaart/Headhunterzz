using System;
using Combat;
using Items;
using UnityEngine;
using UnityEngine.Events;

namespace Characters {

	[RequireComponent(typeof(Sprite))]
	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(Rigidbody2D))]
	[RequireComponent(typeof(BoxCollider2D))]
	public class Character : MonoBehaviour, IAttackable, IDestructible {
		public const float DefaultMoveSpeed = 2.0f;

		public static readonly string[] staticDirections =
			{"Static N", "Static NW", "Static W", "Static SW", "Static S", "Static SE", "Static E", "Static NE"};

		public static readonly string[] runDirections =
			{"Run N", "Run NW", "Run W", "Run SW", "Run S", "Run SE", "Run E", "Run NE"};

		protected Animator _animator;
		protected Rigidbody2D _rigidbody;

		[SerializeField] private CharacterStats statsTemplate;
		[HideInInspector] public CharacterStats stats;
		[HideInInspector] public int lastDirection;

		[HideInInspector] public UnityEvent onPlayerDeath;
		[HideInInspector] public UnityEvent<int> onMobKilled;

		protected virtual void Awake()
		{
			if (statsTemplate == null) throw new NullReferenceException();

			stats = Instantiate(statsTemplate);
			stats.Restart();
		}

		protected virtual void Start() {
			_animator = GetComponent<Animator>();
			_rigidbody = GetComponent<Rigidbody2D>();
		}

		public void SetDirection(Vector2 direction) {
			//use the Run states by default
			string[] directionArray = null;

			//measure the magnitude of the input.
			if (direction.magnitude < .01f) {
				//if we are basically standing still, we'll use the Static states
				//we won't be able to calculate a direction if the user isn't pressing one, anyway!
				directionArray = staticDirections;
			}
			else {
				//we can calculate which direction we are going in
				//use DirectionToIndex to get the index of the slice from the direction vector
				//save the answer to lastDirection
				directionArray = runDirections;
				lastDirection = DirectionToIndex(direction);
			}

			//tell the animator to play the requested state
			_animator.Play(directionArray[lastDirection]);
		}

		public static int DirectionToIndex(Vector2 dir)
		{
			const int sliceCount = 8;
			//get the normalized direction
			Vector2 normDir = dir.normalized;
			//calculate how many degrees one slice is
			float step = 360f / sliceCount;
			//calculate how many degress half a slice is.
			//we need this to offset the pie, so that the North (UP) slice is aligned in the center
			float halfstep = step / 2;
			//get the angle from -180 to 180 of the direction vector relative to the Up vector.
			//this will return the angle between dir and North.
			float angle = Vector2.SignedAngle(Vector2.up, normDir);
			//add the halfslice offset
			angle += halfstep;
			//if angle is negative, then let's make it positive by adding 360 to wrap it around.
			if (angle < 0) {
				angle += 360;
			}

			//calculate the amount of steps required to reach this angle
			float stepCount = angle / step;
			//round it, and we have the answer!
			return Mathf.FloorToInt(stepCount);
		}
		
		//this function converts a string array to a int (animator hash) array.
		public static int[] AnimatorStringArrayToHashArray(string[] animationArray) {
			//allocate the same array length for our hash array
			int[] hashArray = new int[animationArray.Length];
			//loop through the string array
			for (int i = 0; i < animationArray.Length; i++) {
				//do the hash and save it to our hash array
				hashArray[i] = Animator.StringToHash(animationArray[i]);
			}
			
			return hashArray;
		}
		
		public void OnAttack(GameObject attacker, Attack attack)
		{
			stats.TakeDamage(attack.Damage);

			if (true)
			{
				if (attack.IsCritical)
					Debug.Log("CRITICAL DAMAGE !!");
				Debug.LogFormat("{0} attacked {1} for {2} damage. {3} Health Left", attacker.name, name, attack.Damage, stats.GetHealth());
			}
			
			
			attacker.GetComponent<Character>().stats.RegainHealth((int) (attack.Damage * stats.RegainFromAttack));
			
			if (stats.GetHealth() <= 0)
			{
				OnDestruction(attacker);
			}
		}

		public void OnDestruction(GameObject destroyer)
		{
			if (stats.IsPlayer)
			{
				onPlayerDeath.Invoke();
				Destroy(gameObject);
			}
			else
			{
				// give exp
				var playerCharacter = destroyer.GetComponent<Character>();
				playerCharacter.stats.OnMobKilled(stats);
				playerCharacter.onMobKilled.Invoke(GetComponent<Enemy>().mobId);
				// drop
				GetComponent<ItemDrop>().Drop();
			}
		}
	}

}