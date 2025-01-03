using UnityEngine;

namespace _Project._Scripts.PlayerScripts.Weapons.ComboSystem
{
    [CreateAssetMenu(fileName = "ComboChainSO", menuName = "ComboSystem/ComboChainSO")]
    public class ComboChainSO : ScriptableObject
    {
        [System.Serializable]
        public class ComboChainData
        {
            public string chainName;
            public float  maxComboTime     = 2f;
            public bool   requiresGrounded = true;
            public int    maxComboCount    = 3;
        }

        public ComboChainData chainData;
        public AttackSO[]     startingAttacks;
        public AttackSO[]     attacks;
    }
}