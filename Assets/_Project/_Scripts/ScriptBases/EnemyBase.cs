using UnityEngine;

namespace _Project._Scripts.ScriptBases
{
    public abstract class EnemyBase : MonoBehaviour
    {
        public enum EnemyStrategy
        {
            Aggressive,
            Defensive,
            Fleeing,
            Passive,
        }

        [SerializeField] protected UnityEngine.AI.NavMeshAgent agent;

        protected EnemyStrategy currentStrategy;

        // Abstract properties and methods
        public abstract bool IsInCombat      { get; }
        public abstract bool WantsAggressive { get; } // Changed to be abstract since Enemy1 overrides it
        public abstract void CustomUpdate();
        public abstract void TakeDamage(float damage, Vector3 hitDirection);

        // Public property to access strategy
        public EnemyStrategy Strategy => currentStrategy;

        public virtual void SetStrategy(EnemyStrategy newStrategy)
        {
            currentStrategy = newStrategy;
            if (agent != null)
            {
                agent.ResetPath();
            }
        }
    }
}