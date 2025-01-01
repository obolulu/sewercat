using UnityEngine;

namespace _Project._Scripts.PlayerScripts
{
    public class PlayerJumpingState : BaseState<PlayerController.PlayerState>
    {
        private readonly PlayerController controller;
        
        private float _jumpGravityMultiplier = 0.5f;
        private float jumpStartTime;
        private float elapsedTime;
        private float _jumpReleaseTimer;
        private float _jumpReleaseDelay = 0.1f;
        
        public PlayerJumpingState(PlayerController.PlayerState key, PlayerController controller) : base(key)
        {
            this.controller = controller;
        }


        public override void EnterState()
        {
            controller.Jump();
            jumpStartTime = Time.time;
        }

        public override void ExitState()
        {
            controller.lastJumpTime = Time.time;
        }

        public override void UpdateState()
        {
            controller.Move();
        
            // Apply more gravity when going up and not holding jump
            float gravityMultiplier = (!InputManager.State.IsJumping && controller.verticalVelocity.y > 0) 
                ? 1.5f    // Faster fall when releasing jump
                : _jumpGravityMultiplier;
            
            controller.ApplyGravity(controller.gravity * gravityMultiplier);
        }

        public override PlayerController.PlayerState GetNextState()
        {
            elapsedTime = Time.time - jumpStartTime;
            if (controller.verticalVelocity.y > 0 && InputManager.State.IsJumping)
            {
                _jumpReleaseTimer += Time.deltaTime;
                if (_jumpReleaseTimer < _jumpReleaseDelay) return StateKey;
            }

            elapsedTime = Time.time - jumpStartTime;
            if (controller.IsGrounded() && elapsedTime > controller.minJumpTime)
                return PlayerController.PlayerState.Idle;
            if (elapsedTime > controller.maxJumpTime 
                || controller.verticalVelocity.y < 0)
                return PlayerController.PlayerState.Falling;
            return StateKey;
        }
    }
}