using System;
using Combat;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Characters
{
  
    public class Enemy : Character, IAttackable
    {
        public string State = "";
        public float chaseRadius = 5;
        public float combatRadius = 2;
        public Transform target;

        private Path _path;
        private int currentWayPoint = 0;

        private Seeker _seeker;
        private const float PathGenerationTimer = 0.5f;
        private float currentGenerationTimer = PathGenerationTimer;

        private float distanceToAdvance = 0.5f;

        private void Awake()
        {
            _seeker = GetComponent<Seeker>();
        }

        void FixedUpdate()
        {

            if (Vector3.Distance(transform.position, target.position) < combatRadius)
            {
               OnEnterCombat(); 
            }
            
            currentGenerationTimer += Time.fixedDeltaTime;
            if (currentGenerationTimer > PathGenerationTimer)
            {
                if (Vector3.Distance(transform.position, target.position) < chaseRadius)
                {
                    SetToChaseTarget();
                    State = "Chase";
                }
                else
                {
                    SetToWander();
                    State = "Wander";
                }

                currentGenerationTimer = 0.0f;
            }
            

            if (_path == null) return;
            if (currentWayPoint >= _path.vectorPath.Count) return;
            
            Move();
            SetDirection(_rigidbody.velocity);
        }

        private void OnEnterCombat()
        {
            State = "Combat";
        }

        private void OnPathComplete(Path p)
        {
            if (p.error) return;

            _path = p;
            currentWayPoint = 0;
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

        private void Move()
        {
            Vector2 direction = ((Vector2) _path.vectorPath[currentWayPoint] - _rigidbody.position);

            _rigidbody.AddForce(direction, ForceMode2D.Impulse);
            _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, DefaultMoveSpeed);
            float distance = Vector2.Distance(_rigidbody.position, _path.vectorPath[currentWayPoint]);

            if (distance < distanceToAdvance)
            {
                currentWayPoint++;
            }
        }

        public void OnAttack(GameObject attacker, Attack attack)
        {
            if (attack.IsCritical)
                Debug.Log("CRITICAL DAMAGE !!");

            Debug.LogFormat("{0} attacked {1} for {2} damage.", attacker.name, name, attack.Damage);
        }

        private Vector2 PickRandomPoint()
        {
            var point = (Vector2) Random.insideUnitSphere * chaseRadius;
            point += _rigidbody.position;
            return point;
        }
    }
}