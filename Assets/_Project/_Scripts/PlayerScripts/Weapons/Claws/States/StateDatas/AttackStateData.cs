using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;

namespace _Project._Scripts.PlayerScripts.Weapons.Claws.States.StateDatas
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "AttackStateData", menuName = "State Data/Claws/Attack State Data")]
    public class AttackStateData : StateData
    {
        [Header("Attack Data")]
        public  float     attackDamage = 10f;
        public  float attackRange    = 2f;
        public  float attackRadius   = 1f;
        public float attackCooldown = 0.5f;
        public  LayerMask whatIsDamageable;
        
        [Title("Animations & Effects")]
        [Header("Attack Animation")]
        public AttackAnimation attackAnimation;
        
        //[Header("Attack Effects")]
        //public MMFeedbacks attackFeedbacks;
        //public MMFeedbacks hitFeedbacks;
    }
}