using System;
using _Project._Scripts.ScriptBases;
using UnityEngine;

namespace _Project._Scripts.PlayerScripts.Weapons.Claws
{
    public sealed class ComboWeaponInterface : WeaponBase
    {
        [SerializeField] private ComboWeaponFSM<ClawsWeaponFSM.ClawsWeaponState> weapon;
        
        public override void TryAttack()
        {
            weapon.TryAttack();
        }

        public override void Special()
        {
            weapon.Special();
        }

        public override void OnRightClickDown()
        {
            weapon.OnRightClickDown();
        }

        public override void OnRightClickUp()
        {
            weapon.OnRightClickUp();
        }
        
        public override void ResetWeapon()
        {
            weapon.ResetWeaponState();
        } 
    }
}