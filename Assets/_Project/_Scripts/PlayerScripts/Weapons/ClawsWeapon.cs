using System.Collections;
using _Project._Scripts.CameraEffects;
using _Project._Scripts.ScriptBases;
using UnityEngine;
using FMODUnity;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;

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
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }

    #region special / focus 
    public override void Special()
    {
        if (isFocused)
        {
            EndFocus();
        }
        else
        {
            StartFocus();
        }
    }
    
    private void StartFocus()
    {
        isFocused = true;
        //PlayerController.SetFocus(true);
        Debug.Log("Focused");
    }
    
    private void EndFocus()
    {
        isFocused = false;
        //PlayerController.SetFocus(false);
        Debug.Log("Not Focused");
    }
    
    #endregion
    #region blocking/parrying (right click)

    public override void OnRightClickDown()
    {
       StartBlocking();
    }
    public override void OnRightClickUp()
    {
        EndBlocking();
    }
    

    private void StartBlocking()
    {
        PlayerController.SetBlocking(true);
        Debug.Log("Blocking");
    }

    private void EndBlocking()
    {
        PlayerController.SetBlocking(false);
        Debug.Log("Not Blocking");
    }

    private bool IsBlocking()
    {
        return PlayerController.IsBlocking;
    }

    #endregion

}
