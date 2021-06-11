using System;
using Items;
using UnityEngine;
using UnityEngine.Events;

namespace Characters
{
    [CreateAssetMenu(fileName = "NewStats", menuName = "Character/Stats", order = 1)]
    public class CharacterStats : ScriptableObject
    {
        [HideInInspector] public UnityEvent onLevelUp;
        [HideInInspector] public UnityEvent onPlayerDamaged;
        [HideInInspector] public UnityEvent onPlayerHealed;
        [HideInInspector] public UnityEvent onPlayerExperienceGained;

        // unless the object pooling index is set, it is a player
        public bool IsPlayer = false;


        private int _availablePoints = 0;
        private const int PointsPerLevel = 5;

        #region Core Stats
        
        public int Strength;

        public int Stamina;

        public int Luck;

        public int Charisma;

        public int Experience;

        public int Level;

        public int Lifesteal;

        #endregion

        #region Equippable Items

        public ItemDefinition Weapon;

        #endregion
        // TODO Play with the coefficients scaling 
        public int Damage => Strength * 10;
        public int MaxHealth => Stamina * 10;
        public float AdditionalGold => Charisma * 0.2f;
        public float CriticalChance => Luck * 0.01f;

        /** Percent of damage done, regained as health */
        public float RegainFromAttack => Lifesteal * 0.01f;

        private int currentHealth;
        private int gold;

        public void Restart()
        {
            currentHealth = Stamina * 10;
        }

        private void OnLevelUp()
        {
            // leveling up
            Level++;
            // adding points to spend
            _availablePoints += PointsPerLevel;
            // resetting the experience
            Experience = ExperienceTable.GetExperienceAfterLevelUp(Level, Experience);
            // resetting health
            currentHealth = MaxHealth;

            onLevelUp.Invoke();
        }

        public void TakeDamage(int amount)
        {
            currentHealth -= amount;

            if (IsPlayer) onPlayerDamaged.Invoke();
        }

        public void RegainHealth(int amount)
        {
            currentHealth = (currentHealth + amount);
            if (currentHealth > MaxHealth) currentHealth = MaxHealth;

            if (IsPlayer) onPlayerHealed.Invoke();
        }

        public int GetHealth()
        {
            return currentHealth;
        }

        public void OnMobKilled(CharacterStats stats)
        {
            Debug.Log("Enemy killed");
            Experience += stats.Experience;

            if (ExperienceTable.ShouldLevelUp(Level, Experience))
            {
                OnLevelUp();
            }

            onPlayerExperienceGained.Invoke();
        }

        public void GiveGold(int amount)
        {
            gold += amount + (int)(amount * AdditionalGold);
        }

        /** Should be called for the UI */
        public int GetAvailablePoints()
        {
            return _availablePoints;
        }

        #region Events

        public void RegisterOnLevelUpListener(UnityAction listener)
        {
            onLevelUp.AddListener(listener);
        }

        public void RegisterOnDamagedListener(UnityAction listener)
        {
            onPlayerDamaged.AddListener(listener);
        }

        public void RegisterOnGainedHealthListener(UnityAction listener)
        {
            onPlayerHealed.AddListener(listener);
        }

        public void RegisterOnExperienceGainedListener(UnityAction listener)
        {
            onPlayerExperienceGained.AddListener(listener);
        }

        #endregion
    }
}