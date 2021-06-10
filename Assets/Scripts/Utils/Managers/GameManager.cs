using System;
using System.Collections.Generic;
using Characters;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using World;

namespace Utils.Managers
{
    public enum GameState
    {
        WorldMap,
        Island
    }
    
    public class GameManager : Manager<GameManager>
    {
        public GameObject[] systemPrefabs;
        private List<GameObject> instancedSystemPrefabs = new List<GameObject>();

        public GameState CurrentGameState { get; set; } = GameState.WorldMap;
        public UnityEvent<GameState, GameState> onGameStateChanged;
        public UnityEvent<IslandChunk> onIslandVisit;

        [HideInInspector] public Player player;
        
        
        private void Start()
        {
            SetupSystemPrefabs();
        }

        private void SetupSystemPrefabs()
        {
            foreach (var fab in systemPrefabs)
            {
                instancedSystemPrefabs.Add(Instantiate(fab));
            }
        }


        public void UpdateState(GameState nextState)
        {
            GameState previousGameState = CurrentGameState;
            CurrentGameState = nextState;
            
            onGameStateChanged.Invoke(previousGameState, nextState);
        }
        
        public void OnIslandVisit(IslandChunk islandChunk)
        {
            SceneManager.LoadScene("IslandScene");
            
            UpdateState(GameState.Island);

            onIslandVisit.Invoke(islandChunk);
        }

        private void SetupUI()
        {
            UIManager.Instance.UpdatePlayerLevel(player);
            UIManager.Instance.UpdatePlayerExperience(player);
            UIManager.Instance.UpdatePlayerHealth(player);
        }
        
        private void SetupEventListeners()
        {
            player.RegisterOnLevelUpListener(OnPlayerLevelUp);
            player.RegisterOnDamagedListener(OnPlayerDamaged);
            player.RegisterOnGainedHealthListener(OnPlayerHealed);
            player.RegisterOnExperienceGainedListener(OnPlayerGainedExperience);
            player.RegisterOnDeathListener(OnPlayerDeath);
            player.RegisterOnMobKilledListener(OnMobKilled);
        }

        #region Listeners

        public void OnPlayerLevelUp()
        {
            UIManager.Instance.UpdatePlayerLevel(player);
            UIManager.Instance.UpdatePlayerExperience(player);
        }

        public void OnPlayerDamaged()
        {
            UIManager.Instance.UpdatePlayerHealth(player);
        }

        public void OnPlayerHealed()
        {
            UIManager.Instance.UpdatePlayerHealth(player);
        }

        public void OnPlayerGainedExperience()
        {
            UIManager.Instance.UpdatePlayerExperience(player);
        }

        public void OnMobKilled(int mobId)
        {
            SpawnManager.Instance.OnMobKilled(mobId);
        }

        public void OnPlayerDeath()
        {
            foreach (var enemy in FindObjectsOfType<Enemy>())
            {
                enemy.OnPlayerDied();
            }
        }

        public void OnPlayerRespawned()
        {
            foreach (var enemy in FindObjectsOfType<Enemy>())
            {
                enemy.OnPlayerRespawned(player.transform);
            }
        }

        public void InitializePlayer()
        {
            player = FindObjectOfType<Player>();

            SetupEventListeners();
            SetupUI();
        }

        #endregion
    }
}