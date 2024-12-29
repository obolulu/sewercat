using _Project._Scripts.PlayerScripts.Weapons.Claws.States.StateDatas;
using _Project._Scripts.ScriptBases;
using Animancer;
using UnityEngine;

namespace _Project._Scripts.PlayerScripts.Weapons.Claws.States
{
    public class AttackClawState : BaseState<ClawsWeaponFSM.ClawsWeaponState>
    {
        private readonly ClawsWeaponFSM _weaponFSM;
        private          float          attackCooldown;
        private          float          lastAttackTime;
        private          bool           isAttacking;
        private          float          currentAnimationTime;
        
        private          AttackStateData data;
        private AnimancerState currentState;
        private bool eventsAdded;

        public AttackClawState(ClawsWeaponFSM.ClawsWeaponState key, ClawsWeaponFSM weaponFSM, AttackStateData data) :
            base(key)
        {
            _weaponFSM     = weaponFSM;
            this.data      = data;
            attackCooldown = data.attackCooldown;
            lastAttackTime = -attackCooldown;
        }

        public override void EnterState()
        {
            _weaponFSM.ResetState();
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                StartAttack();
            }
        }

        public override void UpdateState()
        {
            if (!isAttacking) return;
            
            if (currentState != null)
            {
                currentAnimationTime = currentState.Time / currentState.Duration;
                
                // Check if we're in the combo window
                if (currentState.NormalizedTime >= data.attackAnimation.earlyComboWindowStart /*&& 
                    currentState.NormalizedTime <= data.attackAnimation.comboWindowEnd*/)
                {
                    // If attack input is received during combo window, buffer it
                    if (_weaponFSM.StateRequest == ClawsWeaponFSM.ClawsWeaponState.Attacking)
                    {
                        data.attackAnimation.BufferInput();
                    }
                }
                
                // When we reach the end of combo window
                if (currentState.NormalizedTime >= data.attackAnimation.comboWindowEnd)
                {
                    // Check if we have a buffered input
                    if (data.attackAnimation.HasBufferedInput)
                    {
                        data.attackAnimation.ConsumeBufferedInput();
                        StartAttack(); // Start the next attack in the combo
                    }
                }
            }
        }

        private void StartAttack()
        {
            _weaponFSM.ResetState();
            isAttacking = true;
            lastAttackTime = Time.time;
            AttackRoutine();
            _weaponFSM.AttackFeedbacks?.PlayFeedbacks();
        }

        private void AttackRoutine()
        {
            currentState = _weaponFSM.Animancer?.Play(data.attackAnimation.attackAnimation);
            if (currentState != null && !eventsAdded)
            {
                currentState.Events(this).OnEnd = EndAttack;
                currentState.Events(this).Add(0.5f, HitDetect); // Add hit detection at 50% of animation
                currentState.Clip.wrapMode = WrapMode.Once; // Prevent looping
                eventsAdded = true;
            }
        }
        
        private void HitDetect()
        {
            Collider[] hitColliders = Physics.OverlapSphere(_weaponFSM.transform.position, 
                                                          data.attackRange, 
                                                          data.whatIsDamageable);

            foreach (Collider hit in hitColliders)
            {
                if (hit.TryGetComponent<IDamageable>(out var enemy))
                {
                    Vector3 hitDirection = (hit.transform.position - _weaponFSM.transform.position).normalized;
                    _weaponFSM.HitFeedbacks?.PlayFeedbacks();
                    enemy.TakeDamage(data.attackDamage, hitDirection);
                }
            }
        }
        
        private void EndAttack()
        {
            isAttacking = false;
            if (!data.attackAnimation.HasBufferedInput)
            {
                _weaponFSM.ResetWeapon();
            }
        }
        
        public override void ExitState()
        {
            isAttacking = false;
            data.attackAnimation.ConsumeBufferedInput(); // Clear any remaining buffered input
            if (currentState != null && eventsAdded)
            {
                currentState.Events(this).Clear();
                eventsAdded = false;
            }
        }

        public override ClawsWeaponFSM.ClawsWeaponState GetNextState()
        {
            if (!isAttacking)
            {
                return ClawsWeaponFSM.ClawsWeaponState.Default;
            }
            return StateKey;
        }
    }
}