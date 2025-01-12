using _Project._Scripts.ScriptBases;
using UnityEngine;

namespace _Project._Scripts.PlayerScripts.Weapons.Matches
{
    public class MatchesInterface: WeaponBase
    {
        [SerializeField] private MatchesWeapon weapon;
        public override void TryAttack()
        {
            weapon.TryAttack();
        }
    }
}