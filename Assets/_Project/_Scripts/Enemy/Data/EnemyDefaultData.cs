using UnityEngine;

namespace _Project._Scripts.EnemyDir
{
    [CreateAssetMenu(fileName = "Enemy Data", menuName = "Game/Enemy Data")]
    public class EnemyDefaultData : ScriptableObject
    {
        [Header("Combat")]
        public float maxHealth = 100f;
        public float disengageRange = 20f;
        public float engageRange    = 10f;
        public float attackRange    = 2f;
        public float attackDamage   = 10f;
        public float attackCooldown = 1.5f;

        [Header("Movement")]
        public float moveSpeed = 5f;
        public float rotationSpeed  = 120f;
        public float patrolWaitTime = 2f;
        
        [Header("Combat Behavior")]
        public float aggroTime = 5f;
        public float stunResistance = 1f;
    }
}