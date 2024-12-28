using UnityEngine;

namespace _Project._Scripts.PlayerScripts
{
    public class PlayerLeapingState : BaseState<PlayerController.PlayerState>
    {
        private readonly PlayerController controller;
        private          float            airControlMultiplier = 0.5f; // How much control player has while leaping
        private          Vector3          leapVelocity;
        
        public PlayerLeapingState(PlayerController.PlayerState key, PlayerController controller) : base(key)
        {
            this.controller = controller;
        }

        public override void EnterState()
        {
            // Store initial leap velocity to blend with player input
            leapVelocity = controller.CurrentMoveVelocity;
        }

        public override void ExitState()
        {
            controller.ResetVerticalVelocity();
        }

        public override void UpdateState()
        {
            // Apply normal movement with reduced control
            controller.Move(airControlMultiplier);
            
            // Apply gravity
            controller.ApplyGravity(controller.gravity);
            
            // Check for obstacles to prevent clipping
            if (controller.IsFacingObstacle())
            {
                controller.SetLeaping(false);
            }
            //if(controller.IsGrounded() && )
        }

        public override PlayerController.PlayerState GetNextState()
        {
            if (!controller.IsLeaping || controller.IsFacingObstacle())
            {
                if (!controller.IsGrounded())
                    return PlayerController.PlayerState.Falling;
                
                return controller.HasMovementInput() 
                    ? PlayerController.PlayerState.Walking 
                    : PlayerController.PlayerState.Idle;
            }
            
            return StateKey;
        }
    }
}