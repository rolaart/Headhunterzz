using System;
using Characters;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.WSA;
using Utils.Attributes;
using World;

namespace Utils.Managers
{
    [Serializable]
    public struct MapGUI
    {
        [SerializeField] public GameObject panel;
        [SerializeField] public TextMeshProUGUI islandName;
        [SerializeField] public TextMeshProUGUI islandChunk;
        [SerializeField] public Button visitIsland;
    }

    [Serializable]
    public struct PlayerFrameGUI
    {
        [SerializeField] public GameObject playerFrame;
        [SerializeField] public Image playerHealthBar;
        [SerializeField] public TextMeshProUGUI playerLevel;
        [SerializeField] public Image playerExperienceBar;
    }

    [Serializable]
    public struct InventoryGUI
    {
        [SerializeField] public GameObject inventoryPanel;

        [NamedArray(new[] {"Helm", "Chest", "Legs", "Feet", "Weapon"})] [SerializeField]
        public Image[] equippedSlots;

        [SerializeField] public Image[] inventorySlots;
        [SerializeField] public Image selectedSlot;
        [SerializeField] public TextMeshProUGUI gold;

        [NamedArray(new[]
        {
            "Strength", "Stamina", "Luck", "Charisma",
            "Damage", "Health", "Crit Chance", "Gold Chance"
        })]
        [SerializeField]
        public TextMeshProUGUI[] characterStats;
    }


    [Serializable]
    public struct IslandDisplayGUI
    {
        [SerializeField] public GameObject islandDisplayPanel;
        [SerializeField] public IslandDisplay islandDisplay;
        [SerializeField] public TextMeshProUGUI islandName;
    }

    public class UIManager : Manager<UIManager>
    {
        [SerializeField] private MapGUI mapGui;
        [SerializeField] private PlayerFrameGUI playerFrameGui;
        [SerializeField] private IslandDisplayGUI islandDisplayGui;
        [SerializeField] private InventoryGUI inventoryGUI;

        private void Start()
        {
            mapGui.visitIsland.onClick.AddListener(() => FindObjectOfType<Map>().TeleportToSelectedIsland());

            islandDisplayGui.islandDisplay.onFadeInAndOutAnimationClip.AddListener(OnIslandDisplayFadeOut);

            GameManager.Instance.onIslandVisit.AddListener(OnIslandVisit);
        }

        #region Listeners

        public void OnIslandDisplayFadeOut()
        {
            islandDisplayGui.islandDisplayPanel.SetActive(false);
        }

        public void OnIslandVisit(IslandChunk islandChunk)
        {
            HideMapGui();
            ShowHUD();

            islandDisplayGui.islandDisplayPanel.SetActive(true);
            islandDisplayGui.islandName.text = islandChunk.Name;
            islandDisplayGui.islandDisplay.FadeIn();
        }

        #endregion

        #region Island

        public void UpdatePlayerHealth(Player player)
        {
            int currentHealth = player.stats.GetHealth();
            int maxHealth = player.stats.MaxHealth;

            playerFrameGui.playerHealthBar.fillAmount = (float) currentHealth / maxHealth;
        }

        public void UpdatePlayerLevel(Player player)
        {
            playerFrameGui.playerLevel.text = player.stats.Level.ToString();
        }

        public void UpdatePlayerExperience(Player player)
        {
            int currentExperience = player.stats.Experience;
            int maxExperience = ExperienceTable.GetExperienceRequired(player.stats.Level);

            playerFrameGui.playerExperienceBar.fillAmount = (float) currentExperience / maxExperience;
        }

        public void ShowHUD()
        {
            playerFrameGui.playerFrame.SetActive(true);
        }

        public void HideHUD()
        {
            playerFrameGui.playerFrame.SetActive(false);
        }

        public void OnInventoryButton(Inventory.Inventory inventory)
        {
            if (inventoryGUI.inventoryPanel.gameObject.activeInHierarchy)
            {
                inventoryGUI.inventoryPanel.SetActive(false);
                return;
            }

            if (true)
            {
                inventory.isDirty = false;

                int rows = inventory.Items.Length;
                int cols = inventory.Items[0].Length;
                // update the inventory
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        bool hasItem = inventory.Items[i][j] != null;
                        int idx = i * cols + j;
                        if (hasItem)
                        {
                            Sprite icon = inventory.Items[i][j].icon;
                            inventoryGUI.inventorySlots[idx].sprite = icon;
                            inventoryGUI.inventorySlots[idx].gameObject.SetActive(true);
                        }
                        else
                        {
                            try
                            {
                                inventoryGUI.inventorySlots[idx].gameObject.SetActive(false);
                            } catch(Exception e)
                            {
                                Debug.Log(idx);
                            }
                           
                        }
                    }
                }

                // update the character equipped items and stats
                var stats = GameManager.Instance.player.stats;
                for (int i = 0; i < stats.EquippedItems.Length; i++)
                {
                    bool hasItem = stats.EquippedItems[i] != null;
                    if (hasItem)
                    {
                        inventoryGUI.equippedSlots[i].sprite = stats.EquippedItems[i].icon;
                        inventoryGUI.equippedSlots[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        inventoryGUI.equippedSlots[i].gameObject.SetActive(false);
                    }
                }

                inventoryGUI.characterStats[0].text = stats.Strength.ToString();
                inventoryGUI.characterStats[1].text = stats.Stamina.ToString();
                inventoryGUI.characterStats[2].text = stats.Luck.ToString();
                inventoryGUI.characterStats[3].text = stats.Charisma.ToString();
                inventoryGUI.characterStats[4].text = stats.Damage.ToString();
                inventoryGUI.characterStats[5].text = stats.MaxHealth.ToString();
                inventoryGUI.characterStats[6].text = stats.CriticalChance.ToString("F") + "%";
                inventoryGUI.characterStats[7].text = stats.AdditionalGold.ToString("F") + "%";
            }

            inventoryGUI.inventoryPanel.SetActive(true);
        }

        #endregion

        #region World Map

        public void ShowMapGui(IslandChunk islandChunk)
        {
            mapGui.panel.SetActive(true);
            mapGui.islandName.text = islandChunk.Name;
            mapGui.islandChunk.text = ((Vector2Int) islandChunk.Position).ToString();
        }

        public void HideMapGui()
        {
            mapGui.panel.SetActive(false);
        }

        #endregion
    }
}