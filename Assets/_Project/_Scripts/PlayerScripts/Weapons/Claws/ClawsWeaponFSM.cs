using _Project._Scripts.PlayerScripts.Weapons.Claws.States;
using _Project._Scripts.PlayerScripts.Weapons.Claws.States.StateDatas;
using Animancer;
using UnityEngine;
using MoreMountains.Feedbacks;

namespace _Project._Scripts.PlayerScripts.Weapons.Claws
{
public sealed class ClawsWeaponFSM : ComboWeaponFSM<ClawsWeaponFSM.ClawsWeaponState>, IComboWeapon
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
    [SerializeField] private ClawLeapStateData leapStateData;
    [SerializeField] private BlockStateData    blockStateData;
    public BlockStateData BlockStateData => blockStateData;
    [Header("Feedbacks")]
    [SerializeField] private MMFeedbacks attackFeedbacks;
    [SerializeField] private MMFeedbacks hitFeedbacks;
    
    [Header("PlayerController")]
    [SerializeField] private PlayerController playerController;
    
    private ClawsWeaponState nextState;
    private Camera mainCamera;
    private float lastAttackTime;

    public bool IsBlocking{ get; set; }
    public ClawsWeaponState StateRequest => nextState; // request from the player controller for the next state
    public PlayerController PlayerController => playerController;
    public AnimancerComponent Animancer       => animator;
    public Camera      MainCamera      => mainCamera;
    public float       LastAttackTime  { get; set; }
    
    private ComboInputType currentInputType;
    public  ComboInputType CurrentInputType => currentInputType;
    

    #region feedbacks

    public MMFeedbacks AttackFeedbacks => attackFeedbacks;
    public MMFeedbacks HitFeedbacks    => hitFeedbacks;

    #endregion

    private void Awake()
    {
        mainCamera = Camera.main;
        playerController = GetComponentInParent<PlayerController>();

        States[ClawsWeaponState.Default] = new DefaultClawState
            (ClawsWeaponState.Default, this);
        States[ClawsWeaponState.Attacking] = new AttackClawState
            (ClawsWeaponState.Attacking, this, attackStateData);
        States[ClawsWeaponState.Blocking] = new BlockingClawState(ClawsWeaponState.Blocking, this);
        States[ClawsWeaponState.Focused] = new FocusedClawState(ClawsWeaponState.Focused, this);
        States[ClawsWeaponState.Leaping] = new LeapingClawState
            (ClawsWeaponState.Leaping, this, leapStateData, playerController);
        
        CurrentState = States[ClawsWeaponState.Default];
    }
    
    
    public void ResetAnimation()
    {
        Animancer.Stop();
    }
    public override void ResetWeaponState()
    {
        nextState        = ClawsWeaponState.Default;
        ResetWeaponInput();
    }

    public void ResetWeaponInput()
    {
        currentInputType = ComboInputType.None;
    }

    public override void TryAttack()
    {
        currentInputType = ComboInputType.LightAttack;
        nextState = ClawsWeaponState.Attacking;
    }

    public override void Special()
    {
        nextState = ClawsWeaponState.Focused;
    }

    public override void OnRightClickDown()
    {
        nextState = ClawsWeaponState.Blocking;
    }

    public override void OnRightClickUp()
    {
        nextState = ClawsWeaponState.Default;
    }
    

}
}