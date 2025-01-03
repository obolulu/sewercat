using _Project._Scripts.ScriptBases;
using Animancer;
using UnityEngine;

namespace _Project._Scripts.PlayerScripts.Weapons.ComboSystem
{
    public interface IComboWeapon
    {
        ComboManager       ComboManager { get; }
        AnimancerComponent Animancer    { get; }
        Transform          Transform    { get; }

        void OnAttackHit(IDamageable target, Vector3 hitPoint);
        void PlayAttackFeedback();
        void PlayHitFeedback();
    }
}