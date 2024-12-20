using System;
using _Project._Scripts.Items;
using _Project._Scripts.PlayerScripts;
using DG.Tweening;
using UnityEngine;

namespace _Project._Scripts.ScriptBases
{
    public class WeaponBase : MonoBehaviour
    {
        [SerializeField] private WeaponData       weaponData;
        protected PlayerController PlayerController;
        
        public virtual           void             TryAttack()        {}
        public virtual           void             Attack()           {}
        public virtual           void             OnRightClickDown() {throw new NotImplementedException();}
        public virtual           void             OnRightClickUp()   {throw new NotImplementedException();}
        
        public void SetWeapon(PlayerController playerController)
        {
            PlayerController = playerController;
        }
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