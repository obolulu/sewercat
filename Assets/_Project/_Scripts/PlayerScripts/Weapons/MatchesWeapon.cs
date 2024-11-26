using _Project._Scripts.ScriptBases;
using UnityEngine;

namespace _Project._Scripts.PlayerScripts.Weapons
{
    public class MatchesWeapon : WeaponBase
    {
        public override void Attack()
        {
            Debug.Log("Attacking with matches");
        }
    }
}