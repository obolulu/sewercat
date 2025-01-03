using _Project._Scripts.ScriptBases;
using UnityEngine;

namespace _Project._Scripts.PlayerScripts.Weapons.ComboSystem
{
    public class ComboWeaponHandler : WeaponBase
    {
        [SerializeField] private ComboManager comboManager;
        private                  bool         hasAttackInput;
        private                  bool         hasSpecialInput;

        private void Awake()
        {
            InputManager.LeftClickDown += OnAttackInput;
            InputManager.Special       += OnSpecialInput;
        }

        private void OnDestroy()
        {
            InputManager.LeftClickDown -= OnAttackInput;
            InputManager.Special       -= OnSpecialInput;
        }

        private void OnAttackInput()
        {
            hasAttackInput = true;
        }

        private void OnSpecialInput()
        {
            hasSpecialInput = true;
        }

        public override void TryAttack()
        {
            hasAttackInput = true;
        }

        public override void Special()
        {
            hasSpecialInput = true;
        }

        public bool ConsumeAttackInput()
        {
            if (hasAttackInput)
            {
                hasAttackInput = false;
                return true;
            }
            return false;
        }

        public bool ConsumeSpecialInput()
        {
            if (hasSpecialInput)
            {
                hasSpecialInput = false;
                return true;
            }
            return false;
        }
    }
}