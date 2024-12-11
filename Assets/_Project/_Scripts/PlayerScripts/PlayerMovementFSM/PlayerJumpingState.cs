using UnityEngine;

namespace _Project._Scripts.PlayerScripts
{
    public class PlayerJumpingState : BaseState<PlayerController.PlayerState>
    {
        private readonly PlayerController controller;
        public PlayerJumpingState(PlayerController.PlayerState key, PlayerController controller) : base(key)
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

        public override PlayerController.PlayerState GetNextState()
        {
            return PlayerController.PlayerState.Falling;
        }
    }
}