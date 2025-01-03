using UnityEngine;

namespace _Project._Scripts.PlayerScripts.Weapons.ComboSystem
{
    [CreateAssetMenu(fileName = "AttackSO", menuName = "ComboSystem/AttackSO")]
    public class AttackSO: ScriptableObject
    {
        [System.Serializable]
        public class AttackData
        {
            public string AttackName;
            public float  AttackDamage;
            public float  AttackRange;
            public bool   IsAoe;
            [Header("Combo Settings")]
            public float comboWindowStart = 0.6f;
            public float     comboWindowEnd = 0.8f;
            public LayerMask whatIsDamageable;
            public Vector3   hitBoxOffset;
            [Header("Attack Properties")]
            public float knockbackForce;
            public float stunDuration;
        }
        
        
        public AttackData    attackData;
        public AnimationClip attackAnimation;
        public FMODEventSO   attackSound;
        [Header("Combo Chain")]
        public AttackSO[] possibleNextAttacks;
        [Header("VFX")]
        public GameObject hitVFX;
        
    }
}