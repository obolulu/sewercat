using _Project._Scripts.PlayerScripts.Weapons.Claws;
using _Project._Scripts.ScriptBases;
using UnityEngine;

namespace _Project._Scripts.PlayerScripts.Weapons.Claws.States{
public class DefaultClawState : BaseState<ClawsWeaponFSM.ClawsWeaponState>
{
    private readonly ClawsWeaponFSM                  _weaponFSM;
    private          ClawsWeaponFSM.ClawsWeaponState nextState;

    public DefaultClawState(ClawsWeaponFSM.ClawsWeaponState key, ClawsWeaponFSM weaponFSM) : base(key)
    {
        _weaponFSM = weaponFSM;
        nextState = ClawsWeaponFSM.ClawsWeaponState.Default;
    }

    public override void EnterState()
    {
        nextState = ClawsWeaponFSM.ClawsWeaponState.Default;
        InputManager.RightClickDown += () =>
            _weaponFSM.TransitionToState(ClawsWeaponFSM.ClawsWeaponState.Blocking);
        InputManager.LeftClickDown += () =>
            _weaponFSM.TransitionToState(ClawsWeaponFSM.ClawsWeaponState.Attacking);
    }

    public override void ExitState()
    {
        InputManager.RightClickDown -= () =>
            _weaponFSM.TransitionToState(ClawsWeaponFSM.ClawsWeaponState.Blocking);
        InputManager.LeftClickDown -= () =>
            _weaponFSM.TransitionToState(ClawsWeaponFSM.ClawsWeaponState.Attacking);
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