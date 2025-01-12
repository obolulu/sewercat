using _Project._Scripts.PlayerScripts.Weapons.Matches.States.Data;
using Animancer;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace _Project._Scripts.PlayerScripts.Weapons.Matches
{
    public enum MatchesWeaponState
    {
        Idle,
        Attacking,
        Special
    }
    public sealed class MatchesWeapon : ComboWeaponFSM<MatchesWeaponState>
    {
        [Header("Animation")]
        [SerializeField] private AnimancerComponent animator;

        [Header("Stats")] 
        [SerializeField] private MatchesFireData attackStateData;
        
        [Header("PlayerController")]
        [SerializeField] private PlayerController playerController;

        [Header("Feedbacks")]
        [SerializeField] private MMFeedbacks attackFeedbacks;
            

        
        public override void TryAttack()
        {
            Debug.Log("attacking with matches");
        }

        public override void Special()
        {
            throw new System.NotImplementedException();
        }

        public override void OnRightClickDown()
        {
            throw new System.NotImplementedException();
        }

        public override void OnRightClickUp()
        {
            throw new System.NotImplementedException();
        }

        public override void ResetWeaponState()
        {
            throw new System.NotImplementedException();
        }
    }
}