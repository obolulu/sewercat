using UnityEngine;

namespace _Project._Scripts.ScriptBases
{
    public interface IDamageable
    {
        public void TakeDamage(float damage, Vector3 hitDirection);
    }
}