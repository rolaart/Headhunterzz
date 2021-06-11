using System;
using Characters;
using Items;
using Unity.VisualScripting;

namespace Inventory
{
    public class Inventory
    {
        private const int RowSize = 6;

        private readonly ItemDefinition[][] items = new ItemDefinition[RowSize][];

        private int nextFreeRow = 0;
        private int nextFreeColumn = 0;

        public Inventory()
        {
            for (int i = 0; i < RowSize; i++)
            {
                items[i] = new ItemDefinition[RowSize];
            }
        }

        public bool CanAdd()
        {
            return nextFreeRow < RowSize || nextFreeColumn < RowSize;
        }

        public void Add(ItemDefinition item, CharacterStats stats)
        {
            if (item.stackable)
            {
                AddStackable(item);
                return;
            }

            // Maybe we can equip it in a weapon or armor slot
            switch (item.type)
            {
                case ItemType.Weapon:
                    if (TryEquip(ref stats.Weapon, item)) return;
                    break;
                case ItemType.Armor:
                    throw new NotImplementedException();
            }

            PlaceInInventory(item);
        }

        private void PlaceInInventory(ItemDefinition item)
        {
            items[nextFreeRow][nextFreeColumn] = item;
            nextFreeColumn++;
            if (nextFreeColumn == RowSize)
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
                    if (items[i][j].type == item.type)
                    {
                        // maybe it is already full stack
                        bool isFullStack = items[i][j].amount == item.maxCountPerStack;
                        if (!isFullStack)
                        {
                            items[i][j].amount += item.amount;
                            int leftover = items[i][j].amount % item.maxCountPerStack;

                            bool hasLeftover = leftover != items[i][j].amount;
                            if (hasLeftover)
                            {
                                items[i][j].amount = item.maxCountPerStack;
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
            if (current) return false;

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
            var tmp = items[x1][y1];
            items[x1][y1] = items[x2][y2];
            items[x2][y2] = tmp;
        }
    }
}