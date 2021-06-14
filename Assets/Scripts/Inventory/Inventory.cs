using System;
using System.Runtime.CompilerServices;
using Characters;
using Items;
using Settings;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils.Managers;

namespace Inventory
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private KeybindsSettings keybindsSettings;
        public bool isDirty = true;
        private const int RowSize = 6;
        private const int ColSize = 4;

        public readonly ItemDefinition[][] Items = new ItemDefinition[RowSize][];
        private int nextFreeRow = 0;
        private int nextFreeColumn = 0;

        private void Start()
        {
            for (int i = 0; i < RowSize; i++)
            {
                Items[i] = new ItemDefinition[ColSize];
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(keybindsSettings.inventoryKey))
            {
                UIManager.Instance.OnInventoryButton(this);
            }
        }

        public bool CanAdd()
        {
            return nextFreeRow < RowSize || nextFreeColumn < ColSize;
        }

        public void Add(ItemPickUp itemPickUp, CharacterStats stats)
        {
            ItemDefinition item = itemPickUp.itemDefinition;
            isDirty = true;
            if (item.stackable)
            {
                AddStackable(item);
                return;
            }

            // Maybe we can equip it in a weapon or armor slot
            bool isEquipped = TryEquip(ref stats.EquippedItems[(int) item.type + (int) item.subType], item);
            if (isEquipped)
            {
                Destroy(itemPickUp.gameObject);
                return;
            }

            PlaceInInventory(item);
            Destroy(itemPickUp.gameObject);
        }

        private void PlaceInInventory(ItemDefinition item)
        {
            Items[nextFreeRow][nextFreeColumn] = item;
            nextFreeColumn++;
            if (nextFreeColumn == ColSize)
            {
                nextFreeColumn = 0;
                nextFreeRow++;
            }
            
        }

        private void AddStackable(ItemDefinition item)
        {
            // we need to find if it is in the inventory first
            for (int i = 0; i < RowSize; i++)
            {
                for (int j = 0; j < RowSize; j++)
                {
                    if (Items[i][j].type == item.type)
                    {
                        // maybe it is already full stack
                        bool isFullStack = Items[i][j].amount == item.maxCountPerStack;
                        if (!isFullStack)
                        {
                            Items[i][j].amount += item.amount;
                            int leftover = Items[i][j].amount % item.maxCountPerStack;

                            bool hasLeftover = leftover != Items[i][j].amount;
                            if (hasLeftover)
                            {
                                Items[i][j].amount = item.maxCountPerStack;
                                item.amount = leftover;
                                PlaceInInventory(item);
                                return;
                            }
                        }
                    }
                }
            }
            
            // if it isnt found in the inventory
            PlaceInInventory(item);
        }

        private static bool TryEquip(ref ItemDefinition current, ItemDefinition toEquip)
        {
            if (current != null) return false;

            current = toEquip;
            return true;
        }

        /** @param x1 - row of first item
         *  @param y1 - col of first item
         *  @param x2 - row of second item
         *  @param y2 - col of second item
         */
        public void SwapItems(int x1, int y1, int x2, int y2)
        {
            var tmp = Items[x1][y1];
            Items[x1][y1] = Items[x2][y2];
            Items[x2][y2] = tmp;
        }
    }
}