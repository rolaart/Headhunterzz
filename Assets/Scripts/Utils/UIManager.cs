using System;
using Characters;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using World;

namespace Utils
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

        public void OnIslandVisit(string visitedIslandName)
        {
            HideMapGui();
            ShowHUD();
            
            islandDisplayGui.islandDisplayPanel.SetActive(true);
            islandDisplayGui.islandName.text = visitedIslandName;
            islandDisplayGui.islandDisplay.FadeIn();

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