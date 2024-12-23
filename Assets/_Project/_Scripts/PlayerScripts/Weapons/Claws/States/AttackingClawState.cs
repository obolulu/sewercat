using _Project._Scripts.PlayerScripts.Weapons.Claws.States.StateDatas;
using _Project._Scripts.ScriptBases;
using UnityEngine;

namespace _Project._Scripts.PlayerScripts.Weapons.Claws.States
{
public class AttackClawState : BaseState<ClawsWeaponFSM.ClawsWeaponState>
{
    private readonly ClawsWeaponFSM _weaponFSM;
    private          float          attackDamage   = 25f;
    private          float          attackRange    = 2f;
    private          float          attackCooldown = 0.5f;
    private          float          lastAttackTime;
    private          LayerMask      enemyLayer;
    private          bool           isAttacking;
    private          StateData      data;

    public AttackClawState(ClawsWeaponFSM.ClawsWeaponState key, ClawsWeaponFSM weaponFSM, StateData data) : base(key)
    {
        _weaponFSM = weaponFSM;
        this.data = data;
        lastAttackTime = -attackCooldown;
    }

    public override void EnterState()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            StartAttack();
        }
    }

    private void StartAttack()
    {
        isAttacking = true;
        lastAttackTime = Time.time;
        //_weaponFSM.WeaponAnimator?.SetTrigger("Attack");
        _weaponFSM.Animancer?.Play(_weaponFSM.AttackAnimation);
        _weaponFSM.SlashEffect?.Play();
        _weaponFSM.AttackFeedbacks?.PlayFeedbacks();
        HitDetect();
    }

    private void HitDetect()
    {
        Collider[] hitColliders = Physics.OverlapSphere(_weaponFSM.transform.position, attackRange, enemyLayer);
        
        foreach (Collider hit in hitColliders)
        {
            if (hit.TryGetComponent<IDamageable>(out var enemy))
            {
                Vector3 hitDirection = (hit.transform.position - _weaponFSM.transform.position).normalized;
                _weaponFSM.HitFeedbacks?.PlayFeedbacks();
                enemy.TakeDamage(attackDamage, hitDirection);
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
        isAttacking = false;
    }

    public override ClawsWeaponFSM.ClawsWeaponState GetNextState()
    {
        return StateKey;
    }
}
}