using System;
using System.Collections.Generic;
using System.Numerics;

namespace _Project._Scripts.PlayerScripts.SaveSystem
{
    [System.Serializable]
    public class SaveData
    {
        public string version;
        public float playerHealth;
        public float playerMana;
        public List<String> inventoryItems;
        public int playerCheckpoint;
    }
}