using UnityEngine;

namespace _Project._Scripts.EnemyDir
{
    [System.Serializable]
    public class EnemySaveData
    {
        public Vector3    position;
        public Quaternion rotation;
        public float   health;
        public bool isDisengaged;
        public bool isInActiveCombat;
    }
}