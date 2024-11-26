using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project._Scripts.Items
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Weapon Item Data")]
    public class WeaponData : ItemData
    {
        public GameObject weaponPrefab;
    }
}