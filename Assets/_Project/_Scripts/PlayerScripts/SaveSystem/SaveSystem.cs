using UnityEngine;
using UnityEngine.Windows;

namespace _Project._Scripts.PlayerScripts.SaveSystem
{
    public class SaveSystem : MonoBehaviour
    {
        private string _savePath;
        public SaveSystem Instance;
        [SerializeField] private GameObject player;
        
        
        
        void Start()
        {
            Instance = this;
            _savePath = Application.persistentDataPath + "/saveData.json";
        }
        
        public void Save(SaveData data)
        {
            string json = JsonUtility.ToJson(data);
            System.IO.File.WriteAllText(_savePath, json);
            Debug.Log("Game Saved");
        }

        public SaveData LoadGame()
        {
            if(File.Exists(_savePath))
            {
                string json = System.IO.File.ReadAllText(_savePath);
                return JsonUtility.FromJson<SaveData>(json);
            }
            {
                Debug.LogError("Save file not found in " + _savePath);
                return null;
            }
        }
        
        
    }
    

}