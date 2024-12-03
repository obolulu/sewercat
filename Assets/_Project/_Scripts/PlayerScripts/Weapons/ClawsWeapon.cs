using _Project._Scripts.ScriptBases;
using UnityEngine;
using FMODUnity;
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
    
    
    [Header("Effects")]
    [SerializeField] private ParticleSystem slashEffect;
    [SerializeField] private EventReference clawAttackSound;
    
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
        AudioManager.Instance.PlaySound("clawAttack", transform.position);
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
