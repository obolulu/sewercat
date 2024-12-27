using UnityEngine;

namespace _Project._Scripts.PlayerScripts
{
    public class PlayerFallingState: BaseState<PlayerController.PlayerState>
    {
        private readonly PlayerController controller;
        
        private float _fallGravityMultiplier = 1.5f;
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
            controller.ResetJump();
        }

        public override void UpdateState()
        {
            controller.Move(0.8f);
            controller.verticalVelocity.y += controller.gravity * _fallGravityMultiplier * Time.deltaTime;
            controller.ApplyGravity(controller.gravity *_fallGravityMultiplier);
        }
    
        public override PlayerController.PlayerState GetNextState()
        {
            if (controller.IsGrounded())
            {
                controller.ResetJump();

                if (controller.CheckJump()) 
                    return PlayerController.PlayerState.Jumping;
                
                if (controller.CheckCrouch())
                    return PlayerController.PlayerState.Crouching;
                
                return controller.HasMovementInput()
                    ? PlayerController.PlayerState.Walking 
                    : PlayerController.PlayerState.Idle;
            }
            return StateKey;
        }
    }
}