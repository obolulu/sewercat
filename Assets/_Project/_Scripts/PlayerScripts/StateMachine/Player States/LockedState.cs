using UnityEngine;

namespace Scripts.Player_Scripts.Player_States
{
    public class LockedState : BaseState<PlayerStateMachine.PlayerState>
    {
        private PlayerStateMachine _playerStateMachine;
        public LockedState(PlayerStateMachine.PlayerState key, PlayerStateMachine psm) : base(key)
        {
            _playerStateMachine = psm;

        }

        public override void EnterState()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;        
        }

        public override void ExitState()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;             
        }

        public override void UpdateState()
        {
            //throw new System.NotImplementedException();
        }

        public override PlayerStateMachine.PlayerState GetNextState()
        {
            return this.StateKey;
        }

        public override void OnTriggerEnter(Collider other)
        {
            throw new System.NotImplementedException();
        }

        public override void OnTriggerStay(Collider other)
        {
            throw new System.NotImplementedException();
        }

        public override void OnTriggerExit(Collider other)
        {
            throw new System.NotImplementedException();
        }
    }
}