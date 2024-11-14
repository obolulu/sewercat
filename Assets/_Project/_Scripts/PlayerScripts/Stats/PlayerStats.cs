using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project._Scripts.PlayerScripts.Stats
{
    public class PlayerStats
    {
        public PlayerStats()
        {
        }
        [ReadOnly] public float Health { get; set; }
        [ReadOnly] public float Mana { get; set; }

        public float Speed { get; set; }

        public float MaxHealth { get; set; }

        public float MaxMana { get; set; }
        
        public List<string> InventoryItems { get; set; }
    }
}