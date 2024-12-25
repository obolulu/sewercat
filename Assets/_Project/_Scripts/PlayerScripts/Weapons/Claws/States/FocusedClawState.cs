using UnityEngine;

namespace _Project._Scripts.PlayerScripts.Weapons.Claws.States
{
    public class FocusedClawState : BaseState<ClawsWeaponFSM.ClawsWeaponState>
    {
        private readonly ClawsWeaponFSM _weaponFSM;
        private ClawsWeaponFSM.ClawsWeaponState nextState;
        public FocusedClawState(ClawsWeaponFSM.ClawsWeaponState key, ClawsWeaponFSM weaponFSM) : base(key)
        {
            _weaponFSM     = weaponFSM;
        }

        public override void EnterState()
        {
            nextState = ClawsWeaponFSM.ClawsWeaponState.Focused;
            _weaponFSM.ResetState();
        }

        public override void ExitState()
        {
        }

        public override void UpdateState()
        {
            CheckState();
        }
        
        private void CheckState()
        {
            Debug.Log(nextState);
            //if player presses focus key again, go back to default
            if (_weaponFSM.StateRequest == ClawsWeaponFSM.ClawsWeaponState.Focused) 
            {
                nextState = (ClawsWeaponFSM.ClawsWeaponState.Default); return;
            }
            if (_weaponFSM.StateRequest == ClawsWeaponFSM.ClawsWeaponState.Attacking)
            {
                nextState = (ClawsWeaponFSM.ClawsWeaponState.Leaping); return;
            }
        }
        public override ClawsWeaponFSM.ClawsWeaponState GetNextState()
        {
            return nextState;
        }
    }
}