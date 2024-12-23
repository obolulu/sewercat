using _Project._Scripts.PlayerScripts.Weapons.Claws;
using _Project._Scripts.ScriptBases;
using UnityEngine;

namespace _Project._Scripts.PlayerScripts.Weapons.Claws.States{
public class DefaultClawState : BaseState<ClawsWeaponFSM.ClawsWeaponState>
{
    private readonly ClawsWeaponFSM                  _weaponFSM;
    private          ClawsWeaponFSM.ClawsWeaponState nextState;
    
    private System.Action rightClickDownHandler;
    private System.Action leftClickDownHandler;


    public DefaultClawState(ClawsWeaponFSM.ClawsWeaponState key, ClawsWeaponFSM weaponFSM) : base(key)
    {
        _weaponFSM            = weaponFSM;
        nextState             = ClawsWeaponFSM.ClawsWeaponState.Default;
        rightClickDownHandler = () => SetNextState(ClawsWeaponFSM.ClawsWeaponState.Blocking);
        leftClickDownHandler  = () => SetNextState(ClawsWeaponFSM.ClawsWeaponState.Attacking);
    }

    public override void EnterState()
    {
        nextState                   =  ClawsWeaponFSM.ClawsWeaponState.Default;
        InputManager.RightClickDown += () => rightClickDownHandler();
        InputManager.LeftClickDown  += () => leftClickDownHandler();
    }

    public override void ExitState()
    {
        InputManager.RightClickDown -= rightClickDownHandler;
        InputManager.LeftClickDown  -= leftClickDownHandler;
    }

    public override void UpdateState()
    {
        
    }

    private void SetNextState(ClawsWeaponFSM.ClawsWeaponState nextState)
    {
        this.nextState = nextState;
    }

    public override ClawsWeaponFSM.ClawsWeaponState GetNextState()
    {
        return nextState;
    }
}
}