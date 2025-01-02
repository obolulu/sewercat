using UnityEngine;

namespace _Project._Scripts.ScriptBases
{
    public abstract class EnemyBase: MonoBehaviour
    {
        public enum EnemyStrategy
        {
            Aggressive,
            Defensive,
            Fleeing,
            Passive,
        }
        public abstract bool IsInCombat { get; }
        //public abstract bool IsDisengaged { get; }
        public abstract void CustomUpdate();
        public abstract void TakeDamage(float damage, Vector3 hitDirection);
    }
}