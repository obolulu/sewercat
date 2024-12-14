using UnityEngine;

namespace _Project._Scripts.PlayerScripts
{
    public class PlayerIdleState: BaseState<PlayerController.PlayerState>
    {
        private readonly PlayerController controller;
    
        public PlayerIdleState(PlayerController.PlayerState key, PlayerController controller) : base(key)
        {
            this.controller = controller;
        }
        public override void EnterState()
        {
            //controller.ResetVerticalVelocity();
            controller.ResetJump();
        }
    
        public override void ExitState() { }
    
        public override void UpdateState()
        {
            controller.Move();
            controller.ApplyGravity(controller.groundedGravity);
        }
    
        public override PlayerController.PlayerState GetNextState()
        {
            if (!controller.IsGrounded())
                return PlayerController.PlayerState.Falling;
            
            if (controller.CheckCrouch())
            {
                return PlayerController.PlayerState.Crouching;
            }
            if (controller.CheckJump())
            {
                return PlayerController.PlayerState.Jumping;
            }
                
            if (controller.HasMovementInput())
                return PlayerController.PlayerState.Walking;
            
            return StateKey;
        }
    }
}