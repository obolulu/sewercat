using System.Collections;
using _Project._Scripts.CameraEffects;
using _Project._Scripts.ScriptBases;
using UnityEngine;
using FMODUnity;
using MoreMountains.Feedbacks;

public class ClawsWeapon : WeaponBase
{
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
    
    
    [Header("Effects")]
    [SerializeField] private ParticleSystem slashEffect;
    
    [Header("Camera Effects")]
    [SerializeField] private EffectChainData attackEffects;

    [SerializeField] private MMFeedbacks attackFeedbacks;
    [SerializeField] private MMFeedbacks hitFeedbacks;
    
    
    [Header("Audio")]
    [SerializeField] private FMODEventSO clawAttackSound;
    
    private Collider collider;
    private float lastAttackTime;
    
    private void Awake()
    {
        if(!animator)
        {
            animator = GetComponent<Animator>();
        }
        lastAttackTime = -attackCooldown;
    }

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
    }

    private void HitDetect()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);
        
        foreach (Collider hit in hitColliders)
        {
            IDamageable enemy = hit.GetComponent<IDamageable>();
            if (enemy != null)
            {
                Vector3 hitDirection = (hit.transform.position - transform.position).normalized;
                hitFeedbacks?.PlayFeedbacks();
                enemy.TakeDamage(attackDamage, hitDirection);
            }
        }    
    }
    
    private void OnDrawGizmosSelected()
    {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public override void TryAttack()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }
}
