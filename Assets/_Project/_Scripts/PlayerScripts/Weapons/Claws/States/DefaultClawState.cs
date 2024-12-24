using _Project._Scripts.PlayerScripts.Weapons.Claws;
using _Project._Scripts.ScriptBases;
using UnityEngine;

namespace _Project._Scripts.PlayerScripts.Weapons.Claws.States{
public class DefaultClawState : BaseState<ClawsWeaponFSM.ClawsWeaponState>
{
    private readonly ClawsWeaponFSM                  _weaponFSM;


    public DefaultClawState(ClawsWeaponFSM.ClawsWeaponState key, ClawsWeaponFSM weaponFSM) : base(key)
    {
        _weaponFSM            = weaponFSM;
    }

    public override void EnterState() { }

    public override void ExitState() { }

    public override void UpdateState() { }

    public override ClawsWeaponFSM.ClawsWeaponState GetNextState()
    {
        var toReturn = _weaponFSM.StateRequest;
        return toReturn;
    }
}
}