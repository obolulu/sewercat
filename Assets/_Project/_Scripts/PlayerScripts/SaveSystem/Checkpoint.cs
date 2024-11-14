using UnityEngine;

namespace _Project._Scripts.PlayerScripts.SaveSystem
{
    public class Checkpoint : MonoBehaviour
    {
        public int checkpointID;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                SaveGameAtCheckpoint(checkpointID);
            }
        }

        private void SaveGameAtCheckpoint(int cpID)
        {
            SaveSystem.Instance.SaveData(cpID);
        }
    }
}