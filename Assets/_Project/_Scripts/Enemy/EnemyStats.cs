using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Enemy/Enemy Data")]
public class EnemyStats : ScriptableObject
{
    [Header("Basic Stats")]
    public float maxHealth = 100f;
    public float moveSpeed = 5f;
    
    [Header("Attack Properties")]
    public float attackDamage = 10f;
    public float attackRange    = 2f;
    public float attackCooldown = 2f;
    public float knockbackForce = 5f;
    
    [Header("Chase Properties")]
    public float chaseRange = 10f;
    public float stoppingDistance = 1.5f;
}