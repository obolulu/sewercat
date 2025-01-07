using _Project._Scripts.PlayerScripts.Weapons.Claws.States.StateDatas;
using Animancer;

namespace _Project._Scripts.PlayerScripts.Weapons.Claws.States
{
    public class BlockingClawState : BaseState<ClawsWeaponFSM.ClawsWeaponState>
    {
        private readonly ClawsWeaponFSM                  _weaponFSM;
        private          AnimancerState                  _blockAnimationState;
        private          ClawsWeaponFSM.ClawsWeaponState nextState;
        
        private          bool                            _isBlocking;


        public BlockingClawState(ClawsWeaponFSM.ClawsWeaponState key, ClawsWeaponFSM weaponFSM) : base(key)
        {
            _weaponFSM = weaponFSM;
        }

        public override void EnterState()
        {
            _isBlocking = true;
            nextState = StateKey;
            _weaponFSM.PlayerController.SetBlocking(true);
            _blockAnimationState = _weaponFSM.Animancer.Play(_weaponFSM.BlockStateData.blockAnimation);
        }

        public override void ExitState()
        {
            _isBlocking = false;
            _weaponFSM.PlayerController.SetBlocking(false);
            _blockAnimationState?.Stop();
        _blockAnimationState = _weaponFSM.Animancer.Play(_weaponFSM.BlockStateData.blockEndAnimation);

        if(_blockAnimationState != null && !_blockAnimationState.HasEvents)
        {
            _blockAnimationState.Events(this).OnEnd = () =>
            {
                //_weaponFSM.ResetState
            };
        }
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
