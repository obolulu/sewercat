using UnityEngine;

namespace _Project._Scripts.PlayerScripts
{
    public class PlayerCrouchingState : BaseState<PlayerController.PlayerState>
    {
        private readonly PlayerController _controller;
        private bool _isTransitioning;
        private bool _blockedByObstacle;
        
        public PlayerCrouchingState(PlayerController.PlayerState key, PlayerController controller) : base(key)
        {
            _controller = controller;
        }

        public override void EnterState()
        {
            _isTransitioning = true;
            AttemptCrouch();
        }

        public override void ExitState()
        {
            if (!_blockedByObstacle)
            {
                AttemptStand();
            }
            _isTransitioning = false;
            _blockedByObstacle = false;
        }

        public override void UpdateState()
        {
            // Apply reduced movement speed while crouching
            _controller.Move(_controller.CrouchSpeedMultiplier);

            // If we're trying to stand up, continuously check if there's clearance
            if (_blockedByObstacle)
            {
                CheckForStandingClearance();
            }
        }

        public override PlayerController.PlayerState GetNextState()
        {
            // Don't allow state changes while height transition is in progress
            if (_isTransitioning) return StateKey;

            // If we're not actively crouching anymore and have clearance to stand
            if (!_controller.CheckCrouch() && !_blockedByObstacle)
            {
                return _controller.HasMovementInput() 
                    ? PlayerController.PlayerState.Walking 
                    : PlayerController.PlayerState.Idle;
            }

            // Handle falling while crouched
            if (!_controller.IsGrounded())
            {
                return PlayerController.PlayerState.Falling;
            }

            // Optional: Handle jumping while crouched if you want to allow it
            // if (_controller.CheckJump()) return PlayerController.PlayerState.Jumping;

            return StateKey;
        }

        private void AttemptCrouch()
        {
            // Start the crouch animation/transition
            _controller.Crouch();
            
            // After animation starts, mark as not transitioning
            _isTransitioning = false;
        }

        private void AttemptStand()
        {
            if (HasClearanceToStand())
            {
                _controller.UnCrouch();
                _blockedByObstacle = false;
            }
            else
            {
                _blockedByObstacle = true;
            }
        }

        private bool HasClearanceToStand()
        {
            // Cast a capsule to check if we have room to stand
            float radius = _controller.characterController.radius;
            Vector3 bottom = _controller.transform.position + Vector3.up * radius;
            Vector3 top = bottom + Vector3.up * (_controller.characterController.height - radius * 2);
            
            // Use a slightly smaller radius for the check to prevent getting stuck on edges
            const float clearanceBuffer = 0.1f;
            
            return !Physics.CapsuleCast(
                bottom,
                top,
                radius - clearanceBuffer,
                Vector3.up,
                out _,
                _controller.characterController.height - _controller.CrouchHeight,
                _controller.groundMask
            );
        }

        private void CheckForStandingClearance()
        {
            if (HasClearanceToStand())
            {
                _blockedByObstacle = false;
                AttemptStand();
            }
        }
    }
}