using System;
using Combat;
using Common;
using Pathfinding;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Characters
{
    public class Enemy : Character
    {
        [SerializeField] private EnemyState state = EnemyState.Wander;
        public float chaseRadius = 5;
        public float combatRadius = 2;
        public float wanderRadius = 1;

        public Transform target;
        public AttackDefinition attack;
        private bool playerIsAlive = true;

        private float timeOfLastAttack = float.MinValue;
        private float timeOfLastWander = float.MinValue;
        private float timeOfLastSeek = float.MinValue;

        private const float PathGenerationCooldown = 0.5f;
        private const float WanderCooldown = 3.0f;

        private Seeker _seeker;
        private Path _path;

        private int currentWayPoint;
        private float distanceToAdvanceWayPoint = 0.5f;

        private Vector2 lastWanderDirection = Vector2.zero;


        protected override void Awake()
        {
            base.Awake();
            _seeker = GetComponent<Seeker>();
        }

        void FixedUpdate()
        {
            if (!playerIsAlive) return;
            
            UpdateState();
            Move();
            if(state != EnemyState.Combat)
                SetDirection(_rigidbody.velocity);
        }
        
        

        private void OnCombat(float distanceFromPlayer)
        {
            state = EnemyState.Combat;
            // set to always face the player when in combat
            gameObject.SetToFaceTarget(target.gameObject);
            
            
            float timeSinceLastAttack = Time.time - timeOfLastAttack;
            bool canAttack = timeSinceLastAttack > attack.Cooldown;

            // we need to get in range
            if (distanceFromPlayer > attack.Range && canAttack)
            {
                SetToChaseTarget();
                return;
            }
            
            // if we are in range and can attack
            if (canAttack)
            {
                Hit();
            }
            else
            {
                // we cant attack so we run away
                SetToRunAway();
            }
        }

        private void OnPathComplete(Path p)
        {
            if (p.error) return;

            _path = p;
            currentWayPoint = 0;
        }

        public void OnPlayerDied()
        {
            _rigidbody.velocity = Vector2.zero;
            playerIsAlive = false;
        }

        public void OnPlayerRespawned(Transform newTarget)
        {
            target = newTarget;
            playerIsAlive = true;
        }

        private void SetToChaseTarget()
        {
            _seeker.StartPath(_rigidbody.position, target.position, OnPathComplete);
        }

        private void SetToWander()
        {
            Vector2 destination = PickRandomPoint();
            _seeker.StartPath(_rigidbody.position, destination, OnPathComplete);
        }

        private void SetToRunAway()
        {
            Vector3 oppositeDirection = ((Vector3) _rigidbody.position - target.position) * -1;
            // oppositeDirection = Vector3.ClampMagnitude(oppositeDirection, 2);
            _seeker.StartPath(_rigidbody.position, oppositeDirection, OnPathComplete);
        }

        private void UpdateState()
        {
            float distanceFromPlayer = Vector3.Distance(transform.position, target.position);
            bool shouldCombat = state == EnemyState.Combat | distanceFromPlayer < combatRadius;

            if (shouldCombat)
            {
                OnCombat(distanceFromPlayer);
                return;
            }

            float timeSinceLastSeek = Time.time - timeOfLastSeek;
            bool canSeek = timeSinceLastSeek > PathGenerationCooldown;

            if (canSeek)
            {
                // is in chase radius
                if (distanceFromPlayer < chaseRadius)
                {
                    SetToChaseTarget();
                    state = EnemyState.Chase;
                    timeOfLastSeek = Time.time;
                }
                else
                {
                    float timeSinceLastWander = Time.time - timeOfLastWander;
                    bool canWander = timeSinceLastWander > WanderCooldown;

                    if (canWander)
                    {
                        Debug.Log("New Wander");
                        SetToWander();
                        state = EnemyState.Wander;
                        timeOfLastWander = Time.time;
                        timeOfLastSeek = Time.time;
                    }
                }
            }
        }
        
        private void Move()
        {
            if (_path == null) return;
            if (currentWayPoint >= _path.vectorPath.Count) return;
            
            Vector2 direction = ((Vector2) _path.vectorPath[currentWayPoint] - _rigidbody.position);

            _rigidbody.AddForce(direction, ForceMode2D.Impulse);
            _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, DefaultMoveSpeed);
            float distance = Vector2.Distance(_rigidbody.position, _path.vectorPath[currentWayPoint]);

            if (distance < distanceToAdvanceWayPoint)
            {
                currentWayPoint++;
            }
        }

        private void Hit()
        {
            SoundManager.Instance.Play(SoundType.SoundWeaponAttack);
            
            timeOfLastAttack = Time.time;
            ((Weapon) attack).ExecuteAttack(gameObject, target.gameObject);
        }

        private Vector2 PickRandomPoint()
        {
            Vector2 point;
            do
            {
                point = Random.insideUnitCircle * wanderRadius;
            } while (point.x * lastWanderDirection.x > 0 && point.y * lastWanderDirection.y > 0);

            lastWanderDirection = point;
            return point + _rigidbody.position;
        }
    }
}