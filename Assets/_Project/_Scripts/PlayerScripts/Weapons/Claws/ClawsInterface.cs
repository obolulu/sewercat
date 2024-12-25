using System;
using _Project._Scripts.ScriptBases;
using UnityEngine;

namespace _Project._Scripts.PlayerScripts.Weapons.Claws
{
    public sealed class WeaponInterface : WeaponBase
    {
        [SerializeField] private ClawsWeaponFSM stateManager;
        
        public override void SetWeapon(PlayerController playerController)
        {
            base.SetWeapon(playerController);
        }

        public void OnDestroy()
        {
            //throw new NotImplementedException();
        }
        
        public override void TryAttack()
        {
            stateManager.TryAttack();
        }

        public override void Special()
        {
            stateManager.Special();
        }

        public override void OnRightClickDown()
        {
            stateManager.OnRightClickDown();
        }

        public override void OnRightClickUp()
        {
            stateManager.OnRightClickUp();
        }
        
        public override void ResetWeapon()
        {
            stateManager.ResetWeapon();
        } 
    }
}