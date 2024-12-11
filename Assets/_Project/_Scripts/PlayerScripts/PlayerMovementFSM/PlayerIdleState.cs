using UnityEngine;

namespace _Project._Scripts.PlayerScripts
{
    public class PlayerIdleState: BaseState<PlayerStateMachine.PlayerState>
    {
        private readonly PlayerController controller;
    
        public PlayerIdleState(PlayerStateMachine.PlayerState key, PlayerController controller) : base(key)
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
    
        public override PlayerStateMachine.PlayerState GetNextState()
        {
            if (!controller.IsGrounded())
                return PlayerStateMachine.PlayerState.Falling;

            if (isJumping == true)
            {
                isJumping = false;
                return PlayerStateMachine.PlayerState.Jumping;
            }
                
            if (controller.HasMovementInput())
                return PlayerStateMachine.PlayerState.Walking;
            
            return StateKey;
        }
    }
}