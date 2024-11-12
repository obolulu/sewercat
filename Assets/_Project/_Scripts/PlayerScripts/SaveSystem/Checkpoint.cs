using System;
using UnityEngine;

namespace _Project._Scripts.PlayerScripts.SaveSystem
{
    public class Checkpoint : MonoBehaviour
    {
        [SerializeField] private SaveSystem saveSystem;
        
        public int checkpointID;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                SaveGameAtCheckpoint();
            }
        }

        private void SaveGameAtCheckpoint()
        {
            SaveData data = new SaveData
            {
                version = "0.1",
                playerHealth = 100,
                playerMana = 100,
                inventoryItems = new System.Collections.Generic.List<string>(),
            };
        }
    }
}