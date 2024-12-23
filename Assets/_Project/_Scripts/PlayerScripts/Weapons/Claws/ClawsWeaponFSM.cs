using _Project._Scripts.PlayerScripts.Weapons.Claws.States;
using _Project._Scripts.PlayerScripts.Weapons.Claws.States.StateDatas;
using Animancer;
using UnityEngine;
using MoreMountains.Feedbacks;

namespace _Project._Scripts.PlayerScripts.Weapons.Claws
{
public sealed class ClawsWeaponFSM : StateManager<ClawsWeaponFSM.ClawsWeaponState> //, IWeapon
{
    public enum ClawsWeaponState
    {
        Default,
        Attacking,
        Blocking,
        Focused,
        Leaping
    }
    
    [Header("Animation")]
    [SerializeField] private AnimancerComponent animator;

    [Header("Stats")] 
    [SerializeField] private AttackStateData attackStateData;
    
    [Header("Feedbacks")]
    [SerializeField] private MMFeedbacks attackFeedbacks;
    [SerializeField] private MMFeedbacks hitFeedbacks;
    
    private ClawsWeaponState nextState;
    private Camera mainCamera;
    private float lastAttackTime;

    
    public MMFeedbacks AttackFeedbacks => attackFeedbacks;
    public MMFeedbacks HitFeedbacks    => hitFeedbacks;
    public AnimancerComponent Animancer      => animator;
    public Camera             MainCamera     => mainCamera;
    public float              LastAttackTime { get; set; }
    private void Awake()
    {
        mainCamera = Camera.main;

        States[ClawsWeaponState.Default] = new DefaultClawState
            (ClawsWeaponState.Default, this);
        States[ClawsWeaponState.Attacking] = new AttackClawState
            (ClawsWeaponState.Attacking, this, attackStateData);
        //States[ClawsWeaponState.Blocking] = new BlockingClawState(ClawsWeaponState.Blocking, this);
        //States[ClawsWeaponState.Focused] = new FocusedClawState(ClawsWeaponState.Focused, this);
        //States[ClawsWeaponState.Leaping] = new LeapingClawState(ClawsWeaponState.Leaping, this);
        
        CurrentState = States[ClawsWeaponState.Default];
    }
}
}