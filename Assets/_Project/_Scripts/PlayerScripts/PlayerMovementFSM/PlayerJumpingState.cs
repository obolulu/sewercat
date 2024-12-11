using UnityEngine;

namespace _Project._Scripts.PlayerScripts
{
    public class PlayerJumpingState : BaseState<PlayerStateMachine.PlayerState>
    {
        private readonly PlayerController controller;
        public PlayerJumpingState(PlayerStateMachine.PlayerState key, PlayerController controller) : base(key)
        {
            this.controller = controller;
        }


        public override void EnterState()
        {
            controller.Jump();
        }

        public override void ExitState()
        {
        }

        public override void UpdateState()
        {
            controller.Move();
            controller.ApplyGravity(controller.gravity);
        }

        public override PlayerStateMachine.PlayerState GetNextState()
        {
            return PlayerStateMachine.PlayerState.Falling;
        }
    }
}