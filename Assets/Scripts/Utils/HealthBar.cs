using System;
using Characters;
using UnityEngine;

namespace Utils
{
    public class HealthBar : MonoBehaviour
    {
        private CharacterStats _characterStats;

        private Vector3 localScale;

        public void Awake()
        {
            _characterStats = GetComponentInParent<Character>().stats;
            localScale = transform.localScale;
        }

        public void Update()
        {
            localScale.x = (float) _characterStats.GetHealth() / _characterStats.MaxHealth;
            transform.localScale = localScale;
        }
    }
}