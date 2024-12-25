using Sirenix.OdinInspector;

namespace _Project._Scripts.PlayerScripts.Weapons.Claws.States.StateDatas
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "ClawLeapStateData", menuName = "State Data/Claws/Claw Leap State Data")]
    public class ClawLeapStateData : StateData
    {
        [Header("Attack Data")]
        public  float    attackDamage = 10f;
        public float     attackRange    = 2f;
        public float     attackRadius   = 1f;
        public float     attackCooldown = 0.5f;
        public LayerMask whatIsDamageable;
        [Header("Leap Data")]
        public float     leapDuration = 0.4f;
        public float     leapDistance = 5f;
        public float     leapHeight   = 0.5f;
        
        [Title("Animations & Effects")]
        [Header("Attack Animation")]
        public AnimationClip leapAnimation;
        //[Header("Attack Effects")]
        //public MMFeedbacks attackFeedbacks;
        //public MMFeedbacks hitFeedbacks;
    }
}