using _Project._Scripts.PlayerScripts.Weapons.Claws.States;
using _Project._Scripts.PlayerScripts.Weapons.Claws.States.StateDatas;
using Animancer;
using UnityEngine;
using MoreMountains.Feedbacks;

namespace _Project._Scripts.PlayerScripts.Weapons.Claws
{
public class ClawsWeaponFSM : StateManager<ClawsWeaponFSM.ClawsWeaponState>
{
    public enum ClawsWeaponState
    {
        Default,
        Attacking,
        Blocking,
        Focused,
        Leaping
    }

    [Header("Attack Properties")]
    //[SerializeField] private float attackDamage = 25f;
    //[SerializeField] private float attackRange = 2f;
    //[SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private LayerMask enemyLayer;
    
    [Header("Animation")]
    [SerializeField] private AnimancerComponent animator;
    [SerializeField] private AnimationClip attackAnimation;
    
    [Header("Effects")]
    [SerializeField] private ParticleSystem slashEffect;
    [SerializeField] private MMFeedbacks attackFeedbacks;
    [SerializeField] private MMFeedbacks hitFeedbacks;
    
    [Header("Audio")]
    [SerializeField] private FMODEventSO clawAttackSound;

    [Header("Stats")] 
    [SerializeField] private StateData attackStateData;
    
    private Camera mainCamera;
    private float lastAttackTime;

    private void Awake()
    {
        mainCamera = Camera.main;

        States[ClawsWeaponState.Default] = new DefaultClawState
            (ClawsWeaponState.Default, this);
        States[ClawsWeaponState.Attacking] = new AttackClawState
            (ClawsWeaponState.Attacking, this,attackStateData);
        //States[ClawsWeaponState.Blocking] = new BlockingClawState(ClawsWeaponState.Blocking, this);
        //States[ClawsWeaponState.Focused] = new FocusedClawState(ClawsWeaponState.Focused, this);
        //States[ClawsWeaponState.Leaping] = new LeapingClawState(ClawsWeaponState.Leaping, this);
        
        CurrentState = States[ClawsWeaponState.Default];
    }

    //public float AttackDamage => attackDamage;
    //public float AttackRange => attackRange;
    //public float AttackCooldown => attackCooldown;
    public LayerMask EnemyLayer => enemyLayer;
    public AnimancerComponent Animancer => animator;
    public AnimationClip AttackAnimation => attackAnimation;
    public ParticleSystem SlashEffect => slashEffect;
    public MMFeedbacks AttackFeedbacks => attackFeedbacks;
    public MMFeedbacks HitFeedbacks => hitFeedbacks;
    public Camera MainCamera => mainCamera;
    public float LastAttackTime { get; set; }
}
}