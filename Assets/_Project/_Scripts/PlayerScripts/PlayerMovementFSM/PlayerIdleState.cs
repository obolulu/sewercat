using UnityEngine;

namespace _Project._Scripts.PlayerScripts
{
    public class PlayerIdleState: BaseState<PlayerController.PlayerState>
    {
        private readonly PlayerController controller;
    
        public PlayerIdleState(PlayerController.PlayerState key, PlayerController controller) : base(key)
        {
            InputManager.JumpPressed += () => isJumping = true;
            this.controller = controller;
        }
        private bool isJumping = false;
        public override void EnterState()
        {
            controller.ResetVerticalVelocity();
        }
    
        public override void ExitState() { }
    
        public override void UpdateState()
        {
            controller.ApplyGravity(controller.groundedGravity);
        }
    
        public override PlayerController.PlayerState GetNextState()
        {
            if (!controller.IsGrounded())
                return PlayerController.PlayerState.Falling;

            if (isJumping == true)
            {
                isJumping = false;
                return PlayerController.PlayerState.Jumping;
            }
                
            if (controller.HasMovementInput())
                return PlayerController.PlayerState.Walking;
            
            return StateKey;
        }
    }
}