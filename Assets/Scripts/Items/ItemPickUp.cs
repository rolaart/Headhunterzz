using System;
using Characters;
using UnityEngine;
using Utils.Managers;

namespace Items
{
    public class ItemPickUp : MonoBehaviour
    {
        [HideInInspector] public ItemDefinition itemDefinition;
        
        
        public void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;

            var stats = other.GetComponent<Character>().stats;

            if (itemDefinition.storable)
            {
                GameManager.Instance.player.Inventory.Add(itemDefinition, stats);
            }
            else
            {
                UseItem(stats);
            }
           
            
        }
        
        

        private void UseItem(CharacterStats stats)
        {
            switch (itemDefinition.type)
            {
                case ItemType.Health:
                    stats.RegainHealth(itemDefinition.amount);
                    break;
                case ItemType.Gold:
                    stats.GiveGold(itemDefinition.amount);
                    break;
                default:
                    throw new NotImplementedException();
            }
            
            Destroy(gameObject);
        }
        
    }
}