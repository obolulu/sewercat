using UnityEngine;

namespace _Project._Scripts.PlayerScripts
{
    public class PlayerFallingState: BaseState<PlayerController.PlayerState>
    {
        private readonly PlayerController controller;
        public PlayerFallingState(PlayerController.PlayerState key, PlayerController controller) : base(key)
        {
            this.controller = controller;
        }

        public override void EnterState()
        {
            //if (controller.verticalVelocity.y > 0)
                //controller.ResetVerticalVelocity();
        }

        public override void ExitState()
        {
            //controller.ResetVerticalVelocity();
        }

        public override void UpdateState()
        {
            controller.Move(0.8f);
            controller.ApplyGravity(controller.gravity);
        }
    
        public override PlayerController.PlayerState GetNextState()
        {
            if (controller.IsGrounded())
                return controller.HasMovementInput() ? PlayerController.PlayerState.Walking : PlayerController.PlayerState.Idle;
            
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