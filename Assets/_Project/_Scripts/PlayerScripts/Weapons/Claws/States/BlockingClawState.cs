using Animancer;
using UnityEngine;

namespace _Project._Scripts.PlayerScripts.Weapons.Claws.States
{
    public class BlockingClawState : BaseState<ClawsWeaponFSM.ClawsWeaponState>
    {
        private readonly ClawsWeaponFSM                  _weaponFSM;
        private          AnimancerState                  _blockAnimationState;
        private          bool                            _isBlocking;
        private          ClawsWeaponFSM.ClawsWeaponState nextState;

        public BlockingClawState(ClawsWeaponFSM.ClawsWeaponState key, ClawsWeaponFSM weaponFSM) : base(key)
        {
            _weaponFSM = weaponFSM;
        }

        public override void EnterState()
        {
            _isBlocking = true;
            nextState = StateKey;
            _weaponFSM.PlayerController.SetBlocking(true);
            //_blockAnimationState = _weaponFSM.Animancer.Play(_weaponFSM.BlockAnimation);
        }

        public override void ExitState()
        {
            _isBlocking = false;
            _weaponFSM.PlayerController.SetBlocking(false);
            _blockAnimationState?.Stop();
        }

        public override void UpdateState()
        {
            CheckState();
        }

        private void CheckState()
        {
            if (_weaponFSM.StateRequest == ClawsWeaponFSM.ClawsWeaponState.Default)
                 nextState =ClawsWeaponFSM.ClawsWeaponState.Default;
        }

        public override ClawsWeaponFSM.ClawsWeaponState GetNextState()
        {
            return nextState;
        }
    }
}
