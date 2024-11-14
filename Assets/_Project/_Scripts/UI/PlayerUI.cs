using _Project._Scripts.PlayerScripts.SaveDirectory;
using _Project._Scripts.PlayerScripts.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace _Project._Scripts.UI
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private Slider             healthSlider;
        [SerializeField] private Slider             manaSlider;
        [SerializeField] private PlayerStatsHandler playerStatsHandler;
        
        private                  PlayerStats        playerStats;

        

        private void Start()
        {
            playerStats                        =  playerStatsHandler.getStats();
            playerStatsHandler.OnHealthChanged += UpdateHealthSlider;
            playerStatsHandler.OnManaChanged   += UpdateManaSlider;
            SaveSystem.OnLoad                  += UpdateUI;
            UpdateUI();
        }

        private void UpdateUI()
        {
            playerStats = playerStatsHandler.getStats();
            UpdateHealthSlider();
            UpdateManaSlider();
        }
        private void OnDestroy()
        {
            playerStatsHandler.OnHealthChanged -= UpdateHealthSlider;
            playerStatsHandler.OnManaChanged -= UpdateManaSlider;
        }
        private void UpdateHealthSlider()
        {
            Debug.Log(playerStats.Health / playerStats.MaxHealth);
            healthSlider.value = playerStats.Health / playerStats.MaxHealth;
        }
        private void UpdateManaSlider()
        {
            Debug.Log(playerStats.Mana / playerStats.MaxMana);
            manaSlider.value = playerStats.Mana / playerStats.MaxMana;     
        }


        
        
    }
}