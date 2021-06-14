using UnityEngine;



namespace Items
{
    public enum ItemType
    {
        Armor = 0,
        Health,
        Gold,
        Weapon = 4,
    }

    public enum ItemSubType
    {
        None = 0,
        Helm = 0,
        Chest = 1,
        Legs = 2,
        Feet = 3
    }
    
    [CreateAssetMenu(fileName = "NewItem", menuName = "Pickable Item", order = 1)]
    public class ItemDefinition : ScriptableObject
    {
        public string name;
        public ItemType type;
        public ItemSubType subType;
        public int amount;
        public int spawnChance;
        
        public Sprite icon;
        public Rigidbody2D spawnObject;

        public bool storable;
        public bool stackable;
        public int maxCountPerStack;

    }
}