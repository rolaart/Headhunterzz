using System;
using System.Collections.Generic;
using Characters;
using UnityEngine;
using UnityEngine.Assertions;
using World;
using Random = UnityEngine.Random;

namespace Utils.Managers
{
    public class SpawnManager : Manager<SpawnManager>
    {
        [SerializeField] private GameObject[] enemyPrefabs;

        // Object Pooling
        private GameObject[] currentEnemies;
        private Queue<int> freeEnemies;

        private int minSpawnCount;
        private int maxSpawnCount;
        private float spawnOnDistanceMoved;
        private Vector3 lastSpawnSpot;

        private void Start()
        {
            Assert.IsTrue(enemyPrefabs.Length > 0);
        }

        public void OnIslandVisit(IslandDifficulty islandDifficulty)
        {
            Debug.Log("Spawner OnIslandVisit");
            int total = 0;
            switch (islandDifficulty)
            {
                case IslandDifficulty.Easy:
                    total = 10;
                    spawnOnDistanceMoved = 10;
                    minSpawnCount = 2;
                    maxSpawnCount = 4;
                    break;
                case IslandDifficulty.Medium:
                    total = 15;
                    spawnOnDistanceMoved = 8;
                    minSpawnCount = 3;
                    maxSpawnCount = 6;
                    break;
                case IslandDifficulty.Hard:
                    spawnOnDistanceMoved = 6;
                    minSpawnCount = 4;
                    maxSpawnCount = 8;
                    total = 20;
                    break;
                default:
                    throw new NotImplementedException();
            }

            currentEnemies = new GameObject[total];
            freeEnemies = new Queue<int>(total);

            for (int i = 0; i < currentEnemies.Length; i++)
            {
                currentEnemies[i] = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]);
                currentEnemies[i].SetActive(false);

                freeEnemies.Enqueue(i);
            }

           
        }

        private void Update()
        {

            float distance = Vector3.Distance(GameManager.Instance.player.transform.position, lastSpawnSpot);
            bool shouldSpawn = distance > spawnOnDistanceMoved;
            
            if (shouldSpawn) Spawn();
        }

        private void Spawn()
        {
            Vector3Int playerPos = Island.Instance.tilemap.WorldToCell(GameManager.Instance.player.transform.position);
            
            int enemiesToSpawn = Random.Range(minSpawnCount, maxSpawnCount);
            enemiesToSpawn = Math.Min(freeEnemies.Count, enemiesToSpawn);

            for (int i = 0; i < enemiesToSpawn; i++)
            {
                int nextEnemyId = freeEnemies.Dequeue();

                Vector3Int cellPos = new Vector3Int();
                int tries = 10;
                while (--tries != 0)
                {
                    Vector2 random = new Vector2(Random.Range(10, 20), Random.Range(10, 20));
                    
                    cellPos = Island.Instance.tilemap.WorldToCell(random);
                    cellPos += playerPos;
                    
                    bool isAvailableSpot = Island.Instance.tilemap.HasTile(cellPos);
                    if (isAvailableSpot)
                    {
                        break;
                    }
                }

                var enemy = currentEnemies[nextEnemyId];
                enemy.transform.position = Island.Instance.tilemap.CellToWorld(cellPos);
                enemy.GetComponent<Character>().stats.mobId = nextEnemyId;
                enemy.GetComponent<Enemy>().target = GameManager.Instance.player.transform;
                enemy.SetActive(true);
            }
        }

        public void OnMobKilled(int mobId)
        {
            currentEnemies[mobId].SetActive(false);
            freeEnemies.Enqueue(mobId);
        }

        public void OnPlayerInitialized(Player player)
        {
            // update player pos
            lastSpawnSpot = player.transform.position;
        }
    }
}