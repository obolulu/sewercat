using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project._Scripts.PlayerScripts.Weapons.Claws.States.StateDatas
{

    [CreateAssetMenu(fileName = "AttackStateData", menuName = "State Data/Claws/Attack State Data")]
    public class AttackStateData : StateData
    {
        [Header("Attack Data")]
        public  float attackDamage = 10f;
        public  float attackRange    = 2f;
        public  float attackRadius   = 1f;
        public float attackCooldown = 0.5f;
        public  LayerMask whatIsDamageable;
        
        [Title("Animations & Effects")]
        [Header("Attack Animation")]
        public AttackAnimation defaultAttack;        
        //[OdinSerialize]
        //public Dictionary<string, AttackAnimation> AttackAnimations = new();
        
        //[Header("Attack Effects")]
        //public MMFeedbacks attackFeedbacks;
        //public MMFeedbacks hitFeedbacks;
    }
}