using System.Collections;
using _Project._Scripts.CameraEffects;
using _Project._Scripts.ScriptBases;
using UnityEngine;
using FMODUnity;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;

public class ClawsWeapon : WeaponBase
{
    private enum State
    {
        Default,
        Blocking,
        Focusing,
        Leaping
    }
    [Header("Attack Properties")]
    [SerializeField] private float attackDamage = 25f;
    [SerializeField]             private float attackRange    = 2f;
    [SerializeField]             private float attackCooldown = 0.5f;
    [SerializeField] private LayerMask     enemyLayer;
    
    
    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string attackTriggerName    = "Attack";
    [SerializeField] private string attackSpeedParameter = "AttackSpeed";
    [SerializeField] private int framesUntilAttack = 4;
    
    [Title("Parry")]
    [Header("Parry Settings")]
    [SerializeField] protected float parryWindowDuration = 0.2f;
    [SerializeField] protected float parryStunDuration = 1f;
    [Header("Parry Feedback")]
    [SerializeField] private MMFeedbacks parrySuccessFeedback;
    [SerializeField] private ParticleSystem parryVFX;

    [Header("Effects")]
    [SerializeField] private ParticleSystem slashEffect;
    
    [Header("Camera Effects")]
    //[SerializeField] private EffectChainData attackEffects;
    [SerializeField] private MMFeedbacks attackFeedbacks;
    [SerializeField] private MMFeedbacks hitFeedbacks;
    
    
    [Header("Audio")]
    [SerializeField] private FMODEventSO clawAttackSound;
    
    private Collider collider;
    private float    lastAttackTime;
    private Camera     camera;
    private bool isFocused;
    private State _currentState;
    private void Awake()
    {
        lastAttackTime = -attackCooldown;
        camera = Camera.main;
        
    }

/*
    private void OnDestroy()
    {

    }
*/
    public override void Attack()
    {
        animator?.SetTrigger(attackTriggerName);
        
        if(slashEffect)
            slashEffect?.Play();
        AudioManager.Instance.PlaySound(clawAttackSound, transform.position);
        //if(attackEffects != null)
            //CameraEffectsManager.Instance.PlayChain(attackEffects);
        attackFeedbacks?.PlayFeedbacks();
        Debug.Log("waiting for: " + attackCooldown);
        StartCoroutine(DelayedHitDetect());
    }
    


    private IEnumerator DelayedHitDetect()
    {
        int frameCount = 0;
        while (frameCount < framesUntilAttack)
        {
            frameCount++;
            yield return new WaitForEndOfFrame();
        }
        HitDetect();
        //animator.ResetTrigger(attackTriggerName);
    }

    private void HitDetect()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);
        
        foreach (Collider hit in hitColliders)
        {
            IDamageable enemy = hit.GetComponent<IDamageable>();
            if (IsLineOfSight(hit))
            {
                if (enemy != null)
                {
                    Vector3 hitDirection = (hit.transform.position - transform.position).normalized;
                    hitFeedbacks?.PlayFeedbacks();
                    enemy.TakeDamage(attackDamage, hitDirection);
                }
            }
        }    
    }

    private bool IsLineOfSight(Collider enemy)
    {
        Vector3 direction = enemy.transform.position - camera.transform.position;
        if(Physics.Raycast(camera.transform.position, direction, out RaycastHit hit, attackRange))
        {
            return hit.collider == enemy;
        }
        return false;
    }
    
    private void OnDrawGizmosSelected()
    {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public override void TryAttack()
    {
        
        if (_currentState == State.Default 
            && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }

    #region special / focus 
    public override void Special()
    {
        if (_currentState == State.Default)
        {
            StartFocus();
        }
        else if (_currentState == State.Focusing)
        {
            EndFocus();
        }
    }
    
    private void StartFocus()
    {
        _currentState = State.Focusing;
        Debug.Log("Focused");
    }
    
    private void EndFocus()
    {
        _currentState = State.Default;
        Debug.Log("Not Focused");
    }
    
    #endregion
    
    #region blocking/parrying (right click)

    public override void OnRightClickDown()
    {
        if(_currentState == State.Default)
            StartBlocking();
    }
    public override void OnRightClickUp()
    {
        if(_currentState == State.Blocking)
        EndBlocking();
    }
    

    private void StartBlocking()
    {
        _currentState = State.Blocking;
        PlayerController.SetBlocking(true);
        Debug.Log(_currentState);
    }

    private void EndBlocking()
    {
        _currentState = State.Default;
        PlayerController.SetBlocking(false);
        Debug.Log(_currentState);
    }

    private bool IsBlocking()
    {
        return PlayerController.IsBlocking;
    }

    #endregion

}
