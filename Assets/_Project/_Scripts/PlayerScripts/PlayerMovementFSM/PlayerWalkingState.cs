using UnityEngine;

namespace _Project._Scripts.PlayerScripts
{
    public class PlayerWalkingState : BaseState<PlayerController.PlayerState>
    {
        private readonly PlayerController controller;
        private          bool             isJumping = false;

        public PlayerWalkingState(PlayerController.PlayerState key, PlayerController controller) : base(key)
        {
            this.controller          =  controller;
            InputManager.JumpPressed += () => isJumping = true;
        }

        public override void EnterState()
        {
        }

        public override void ExitState()
        {
        }

        public override void UpdateState()
        {
            controller.Move();
            controller.ApplyGravity(controller.groundedGravity);
        }

        public override PlayerController.PlayerState GetNextState()
        {
            if (!controller.IsGrounded())
            {
                return PlayerController.PlayerState.Falling;
            }

            if (isJumping)
            {
                isJumping = false;
                return PlayerController.PlayerState.Jumping;
            }

            if (!controller.HasMovementInput())
                return PlayerController.PlayerState.Idle;

            return StateKey;
        }
    }
}