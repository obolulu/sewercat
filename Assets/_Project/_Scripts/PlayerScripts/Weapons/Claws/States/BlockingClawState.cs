using Animancer;
using UnityEngine;

namespace _Project._Scripts.PlayerScripts.Weapons.Claws.States
{
    public class BlockingClawState : BaseState<ClawsWeaponFSM.ClawsWeaponState>
    {
        private readonly ClawsWeaponFSM _weaponFSM;
        private AnimancerState _blockAnimationState;
        private bool _isBlocking;

        public BlockingClawState(ClawsWeaponFSM.ClawsWeaponState key, ClawsWeaponFSM weaponFSM) : base(key)
        {
            _weaponFSM = weaponFSM;
        }

        public override void EnterState()
        {
            _isBlocking = true;
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
            
        }

        public override ClawsWeaponFSM.ClawsWeaponState GetNextState()
        {
            return StateKey;
        }
    }
}
