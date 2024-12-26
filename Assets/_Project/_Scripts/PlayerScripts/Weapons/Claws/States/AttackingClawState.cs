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

        private void StartAttack()
        {
            isAttacking    = true;
            lastAttackTime = Time.time;
            AttackRoutine();
        }

        private void AttackRoutine()
        {
            currentState = _weaponFSM.Animancer?.Play(data.attackAnimation);
            //state.Events(this).Add(data.attackAnimation.length, EndAttack);

            if (!eventsAdded)
            {
                currentState.Events(this).OnEnd = EndAttack;
                currentState.Events(this).Add(0.2f, () => { _weaponFSM.AttackFeedbacks?.PlayFeedbacks(); });

                currentState.Events(this).Add(0.5f, () =>
                {
                    //_weaponFSM.HitFeedbacks?.PlayFeedbacks();
                    HitDetect();
                });

            }
        }

        private void EndAttack()
        {
            isAttacking = false;
            _weaponFSM.AttackFeedbacks.StopFeedbacks();
            _weaponFSM.HitFeedbacks.StopFeedbacks();
            _weaponFSM.Animancer.Stop(); // Stop the animation   
            //_weaponFSM.TransitionToState(ClawsWeaponFSM.ClawsWeaponState.Default);
        }

        private void HitDetect()
        {
            Collider[] hitColliders =
                Physics.OverlapSphere(_weaponFSM.transform.position, data.attackRange, data.whatIsDamageable);

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

        public override void UpdateState()
        {
            if (!isAttacking)
            {
                _weaponFSM.TransitionToState(ClawsWeaponFSM.ClawsWeaponState.Default);
            }
        }

        public override void ExitState()
        {
            if (currentState != null)
            {
                currentState.Events(this).Clear();
                currentState = null;
            }
            eventsAdded = false;
            isAttacking = false;
        }

        public override ClawsWeaponFSM.ClawsWeaponState GetNextState()
        {
            if (!isAttacking || (_weaponFSM.Animancer?.IsPlaying(data.attackAnimation) == false))
            {
                return (ClawsWeaponFSM.ClawsWeaponState.Default);
            }

            return StateKey;
        }
    }
}