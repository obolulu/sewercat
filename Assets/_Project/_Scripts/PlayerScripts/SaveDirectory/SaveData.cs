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
        public float playerMaxHealth;
        public float playerMana;
        public float playerMaxMana;
        public List<String> inventoryItems;
        public int playerCheckpoint;
        public UnityEngine.Vector3 playerLocation;
        public List<int> interactedItems;
    }
}