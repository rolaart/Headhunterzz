using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Items
{
    public class ItemDrop : MonoBehaviour, IDroppable
    {
        public ItemDefinition[] itemDefinitions;
        public Rigidbody2D ItemSpawned { get; set; }
        public ItemPickUp ItemPickUp { get; set; }

        private int totalSpawnChance = 0;
        
        private void Start()
        {
            foreach (var itemDefinition in itemDefinitions)
            {
                totalSpawnChance += itemDefinition.spawnChance;
            }
        }

        public void Drop()
        {
            int percent = Random.Range(0, totalSpawnChance);
            int accumulated = 0;
            foreach (var item in itemDefinitions)
            {
                accumulated += item.spawnChance;
                bool isChosen = accumulated >= percent;
                if (isChosen)
                {
                    ItemSpawned = Instantiate(item.spawnObject, transform.position, Quaternion.identity);
                    ItemPickUp = ItemSpawned.GetComponent<ItemPickUp>();
                    ItemPickUp.itemDefinition = item;
                    Debug.Log("Enemy dropped" + item.name);
                    break;
                }

            }
        }
    }
}