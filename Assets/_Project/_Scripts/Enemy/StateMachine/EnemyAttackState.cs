using _Project._Scripts.ScriptBases;
using UnityEngine;
using Animancer;

namespace _Project._Scripts.EnemyDir.StateMachine
{
    public class EnemyAttackState : BaseState<EnemyStateMachine.EnemyState>
    {
        private readonly Transform playerTransform;
        private readonly Transform enemyTransform;
        private readonly float attackRange    = 5f;
        private readonly float attackCooldown = 4.0f;

        private  float lastAttackTime;
        private  LayerMask playerLayer;

        private AnimancerComponent animancer;
        private AnimationClip      attackClip;

        private EnemyStateMachine esm; //enemy state machine

        private float damage = 10;
        private bool isParryable;
        private bool hasHitPlayer;

        public EnemyAttackState(EnemyStateMachine.EnemyState key,            Transform playerTransform,
                                Transform                    enemyTransform, LayerMask playerLayer,
                                AnimancerComponent animancer, AnimationClip attackClip, EnemyStateMachine esm) : base(key)
        {
            this.playerTransform = playerTransform;
            this.enemyTransform  = enemyTransform;
            this.playerLayer     = playerLayer;
            this.animancer       = animancer;
            this.attackClip      = attackClip;
            this.esm = esm;
        }

        public override void EnterState()
        {
            lastAttackTime = Time.time - attackCooldown; // Allow immediate first attack
        }

        public override void ExitState()
        {
        }

        public override void UpdateState()
        {
            if (playerTransform != null && Time.time >= lastAttackTime + attackCooldown)
            {
                lastAttackTime = Time.time;
                StartAttack();
            }
        }

        private void StartAttack()
        {
            PlayAttackAnimation();
        }

        private void PlayAttackAnimation()
        {
            if (animancer != null && attackClip != null)
            {
                hasHitPlayer = false;
                var state = animancer.Play(attackClip);
                state.Events(this).OnEnd = OnAttackAnimationEnd;
                state.Events(this).Add(0.2f, () => 
                {
                    SetParryable(true);
                    esm.AttackFeedbacks.PlayFeedbacks();  // Open parry window at 20% of the animation
                });
                state.Events(this).Add(0.5f, () =>
                {
                    AttackPlayer();
                    SetParryable(false);
                    hasHitPlayer = true;
                }); // Close parry window at 50% of the animation
       


            }
        }
        private void SetParryable(bool value)
        {
            isParryable = value;
        }

        private void OnAttackAnimationEnd()
        {
            esm.AttackFeedbacks.StopFeedbacks();
            animancer.Stop(); // Stop the animation        
        }

        private void AttackPlayer()
        {
            if (hasHitPlayer) return;
            Collider[] hitColliders = Physics.OverlapSphere(enemyTransform.position, attackRange, playerLayer);
            foreach (Collider hit in hitColliders)
            {
                if (!IsLineOfSight(hit)) continue;
                IDamageable target       = hit.GetComponent<IDamageable>();
                Vector3     hitDirection = (hit.transform.position - enemyTransform.position).normalized;
                target.TakeDamage(damage, hitDirection);
                //Debug.Log("Player hit for " + damage + " damage.");
                hasHitPlayer = true;
            }
        }

        private bool IsLineOfSight(Collider target)
        {
            Vector3 direction = target.transform.position - enemyTransform.transform.position;
            if (Physics.Raycast(enemyTransform.transform.position, direction, out RaycastHit hit, attackRange))
            {
                return hit.collider == target;
            }

            return false;
        }

        public override EnemyStateMachine.EnemyState GetNextState()
        {
            if (playerTransform == null) return StateKey;

            float distanceToPlayer = Vector3.Distance(enemyTransform.position, playerTransform.position);

            // If too far, chase
            if (distanceToPlayer > attackRange)
            {
                return EnemyStateMachine.EnemyState.Chase;
            }

            // If attack is on cooldown, go to idle
            if (Time.time < lastAttackTime + attackCooldown)
            {
                return EnemyStateMachine.EnemyState.Idle;
            }

            // Otherwise stay in attack state
            return EnemyStateMachine.EnemyState.Attack;
        }

        public override void OnTriggerEnter(Collider other)
        {
            throw new System.NotImplementedException();
        }

        public override void OnTriggerStay(Collider other)
        {
            throw new System.NotImplementedException();
        }

        public override void OnTriggerExit(Collider other)
        {
            throw new System.NotImplementedException();
        }
    }
}