using System;
using _Project._Scripts.Items;
using DG.Tweening;
using UnityEngine;

namespace _Project._Scripts.ScriptBases
{
    public class WeaponBase : MonoBehaviour
    {
        [SerializeField] private WeaponData weaponData;

        public virtual void Attack(){}

        public void OnEquip(Vector3 visiblePosition, float animationDuration)
        {
            transform.DOLocalMove(visiblePosition, animationDuration)
                      .SetEase(Ease.OutBack);
        }
        
        public void OnUnequip(Vector3 hiddenPosition, float animationDuration)
        {
            transform.DOLocalMove(hiddenPosition, animationDuration)
                      .SetEase(Ease.InBack);
            
        }
    }
}