using UnityEngine;

namespace _Project._Scripts.PlayerScripts
{
    public class PlayerFallingState: BaseState<PlayerStateMachine.PlayerState>
    {
        private readonly PlayerController controller;
        public PlayerFallingState(PlayerStateMachine.PlayerState key, PlayerController controller) : base(key)
        {
            this.controller = controller;
        }

        public override void EnterState()
        {
            throw new System.NotImplementedException();
        }

        public override void ExitState()
        {
            throw new System.NotImplementedException();
        }

        public override void UpdateState()
        {
            controller.Move();
            controller.ApplyGravity(controller.gravity);
        }
    
        public override PlayerStateMachine.PlayerState GetNextState()
        {
            if (controller.IsGrounded())
                return controller.HasMovementInput() ? PlayerStateMachine.PlayerState.Walking : PlayerStateMachine.PlayerState.Idle;
            
            return StateKey;
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