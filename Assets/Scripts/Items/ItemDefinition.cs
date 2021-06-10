using UnityEngine;



namespace Items
{
    public enum ItemType
    {
        Health,
        Gold,
        Weapon,
        Armor
    }

    public enum ItemSubType
    {
        Helm,
        Chest,
        Legs,
        Feet
    }
    
    [CreateAssetMenu(fileName = "NewItem", menuName = "Pickable Item", order = 1)]
    public class ItemDefinition : ScriptableObject
    {
        public string name;
        public ItemType type;
        public ItemSubType subType;
        public int amount;
        public int spawnChance;

        public Material material;
        public Sprite icon;
        public Rigidbody2D spawnObject;

        public bool storable;
        public bool stackable;
        public int maxCountPerStack;

    }
}