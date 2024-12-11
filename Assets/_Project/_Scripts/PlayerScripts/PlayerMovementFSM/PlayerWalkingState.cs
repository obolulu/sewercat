using UnityEngine;

namespace _Project._Scripts.PlayerScripts
{
    public class PlayerWalkingState : BaseState<PlayerStateMachine.PlayerState>
    {
        public PlayerWalkingState(PlayerStateMachine.PlayerState key) : base(key)
        {
        }

        private readonly PlayerController controller;
        private          bool             isJumping = false;

        public PlayerWalkingState(PlayerStateMachine.PlayerState key, PlayerController controller) : base(key)
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

        public override PlayerStateMachine.PlayerState GetNextState()
        {
            if (!controller.IsGrounded())
            {
                return PlayerStateMachine.PlayerState.Falling;
            }

            if (isJumping)
            {
                isJumping = false;
                return PlayerStateMachine.PlayerState.Jumping;
            }

            if (!controller.HasMovementInput())
                return PlayerStateMachine.PlayerState.Idle;

            return StateKey;
        }
    }
}