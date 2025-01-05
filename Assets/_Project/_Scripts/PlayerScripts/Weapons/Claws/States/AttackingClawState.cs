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
        
        private AttackAnimation currentAttackAnimation;
        private AttackStateData data;
        private AnimancerState currentState;
        private bool eventsAdded;

        public AttackClawState(ClawsWeaponFSM.ClawsWeaponState key, ClawsWeaponFSM weaponFSM, AttackStateData data) :
            base(key)
        {
            _weaponFSM             = weaponFSM;
            this.data              = data;
            attackCooldown         = data.attackCooldown;
            lastAttackTime         = -attackCooldown;
            currentAttackAnimation = data.defaultAttack;  // Initialize with default attack
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
                if (currentState.NormalizedTime >= currentAttackAnimation.earlyComboWindowStart)
                {
                    // If attack input is received during combo window, buffer it
                    if (_weaponFSM.StateRequest == ClawsWeaponFSM.ClawsWeaponState.Attacking)
                    {
                        currentAttackAnimation.BufferInput(_weaponFSM.CurrentInputType);
                    }
                }
            
                // When we reach the end of combo window
                if (currentState.NormalizedTime >= currentAttackAnimation.comboWindowEnd)
                {
                    // Check if we have a buffered input and valid next attack
                    if (currentAttackAnimation.TryGetNextAttack(out var nextAttack))
                    {
                        currentAttackAnimation.ConsumeBufferedInput();
                        currentAttackAnimation = nextAttack;
                        StartAttack();
                    }
                }
            }
        }


        private void StartAttack()
        {
            _weaponFSM.ResetState();
            isAttacking    = true;
            lastAttackTime = Time.time;
        
            // Use current attack animation
            currentState = _weaponFSM.Animancer?.Play(currentAttackAnimation.attackAnimation);
            if (currentState != null && !eventsAdded)
            {
                currentState.Events(this).OnEnd = EndAttack;
                currentState.Events(this).Add(0.5f, HitDetect);
                currentState.Clip.wrapMode = WrapMode.Once;
                eventsAdded                = true;
            }
            
            _weaponFSM.AttackFeedbacks?.PlayFeedbacks();
            AudioManager.Instance.PlaySound(currentAttackAnimation?.attackSound, 
                                                  _weaponFSM.transform.position);
        }
        
        private void HitDetect()
        {
            Collider[] hitColliders = Physics.OverlapSphere(_weaponFSM.transform.position, 
                                                          data.attackRange, 
                                                          data.whatIsDamageable);
            Collider closestEnemy    = null;
            float    closestDistance = float.MaxValue;
            
            foreach (Collider hit in hitColliders)
            {
                float distance = Vector3.Distance(_weaponFSM.transform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy    = hit;
                }
                if (closestEnemy.TryGetComponent<IDamageable>(out var enemy))
                {
                    Vector3 hitDirection = (hit.transform.position - _weaponFSM.transform.position).normalized;
                    _weaponFSM.HitFeedbacks?.PlayFeedbacks();
                    enemy.TakeDamage(data.attackDamage * currentAttackAnimation.damageMultiplier, hitDirection);
                }
            }
        }
        
        private void EndAttack()
        {
            isAttacking = false;
            if (!currentAttackAnimation.HasBufferedInput)
            {
                currentAttackAnimation = data.defaultAttack; // Reset to default attack if combo ends
                _weaponFSM.ResetWeapon();
            }
        }
        
        public override void ExitState()
        {
            isAttacking = false;
            currentAttackAnimation.ConsumeBufferedInput();
            currentAttackAnimation = data.defaultAttack; // Reset to default attack
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