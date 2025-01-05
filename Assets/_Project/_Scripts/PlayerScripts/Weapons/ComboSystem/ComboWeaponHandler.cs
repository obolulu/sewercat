using UnityEngine;
using _Project._Scripts.ScriptBases;
using MoreMountains.Feedbacks;

namespace _Project._Scripts.PlayerScripts.Weapons.ComboSystem
{
    public class ComboWeaponHandler : WeaponBase, IWeapon
    {
        [SerializeField] private ComboManager comboManager;
        [SerializeField] private ComboChainSO defaultComboChain;
        [SerializeField] private MMFeedbacks attackFeedbacks;
        [SerializeField] private MMFeedbacks hitFeedbacks;
        
        private bool hasAttackInput;
        private bool hasSpecialInput;
        private bool isBlocking;

        private void Awake()
        {
            // We'll handle input through the IWeapon interface instead
            comboManager.SetComboChain(defaultComboChain);
        }

        public override void SetWeapon(PlayerController playerController)
        {
            base.SetWeapon(playerController);
            // Additional setup if needed
        }

        // IWeapon implementation
        public override void TryAttack()
        {
            hasAttackInput = true;
        }

        public override void Special()
        {
            hasSpecialInput = true;
        }

        public override void OnRightClickDown()
        {
            isBlocking = true;
            // Implement blocking logic or transition to blocking state
        }

        public override void OnRightClickUp()
        {
            isBlocking = false;
            // Reset blocking state
        }

        public override void ResetWeapon()
        {
            hasAttackInput = false;
            hasSpecialInput = false;
            isBlocking = false;
            comboManager.ResetCombo();
        }

        // Input consumption methods
        public bool ConsumeAttackInput()
        {
            if (hasAttackInput/* && !isBlocking*/) // Don't allow attacks while blocking
            {
                Debug.Log("Consuming attack input");
                hasAttackInput = false;
                return true;
            }
            return false;
        }

        public bool ConsumeSpecialInput()
        {
            if (hasSpecialInput && !isBlocking) // Don't allow specials while blocking
            {
                hasSpecialInput = false;
                return true;
            }
            return false;
        }

        // Feedback methods
        public void PlayAttackFeedback()
        {
            attackFeedbacks?.PlayFeedbacks();
        }

        public void PlayHitFeedback()
        {
            hitFeedbacks?.PlayFeedbacks();
        }
    }
}