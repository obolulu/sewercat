using _Project._Scripts.PlayerScripts.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace _Project._Scripts.UI
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Slider manaSlider;
        [SerializeField] private PlayerStats playerStats;

        private void Start()
        {
            UpdateHealthSlider();
            UpdateManaSlider();
        }

        private void UpdateManaSlider()
        {
            healthSlider.value = playerStats.Mana / playerStats.Mana;     
        }

        private void UpdateHealthSlider()
        {
            healthSlider.value = playerStats.Health / playerStats.MaxHealth;
        }
    }
}