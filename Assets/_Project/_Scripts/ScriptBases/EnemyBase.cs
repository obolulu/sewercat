using UnityEngine;

namespace _Project._Scripts.ScriptBases
{
    public abstract class EnemyBase: MonoBehaviour
    {
        public abstract bool IsInActiveCombat { get; }
        public abstract bool IsDisengaged { get; }
        public abstract void CustomUpdate();
        public abstract void TakeDamage(float damage);
    }
}