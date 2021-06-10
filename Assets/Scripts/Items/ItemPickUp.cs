using Characters;
using UnityEngine;
using Utils.Managers;

namespace Items
{
    public class ItemPickUp : MonoBehaviour
    {
        public ItemDefinition itemDefinition;
        
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

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
            }
        }
        
    }
}