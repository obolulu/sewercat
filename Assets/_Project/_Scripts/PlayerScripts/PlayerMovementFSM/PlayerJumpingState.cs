using UnityEngine;

namespace _Project._Scripts.PlayerScripts
{
    public class PlayerJumpingState : BaseState<PlayerController.PlayerState>
    {
        private readonly PlayerController controller;

        private float jumpStartTime;
        private float elapsedTime;
        
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
        }

        public override void UpdateState()
        {
            controller.Move();
            controller.ApplyGravity(0/*controller.gravity*/);
        }

        public override PlayerController.PlayerState GetNextState()
        {
            elapsedTime = Time.time - jumpStartTime;
            if (controller.IsGrounded() && elapsedTime > controller.minJumpTime)
                return PlayerController.PlayerState.Idle;
            if (elapsedTime > controller.maxJumpTime || (elapsedTime > controller.minJumpTime && !InputManager.StartJump))
                return PlayerController.PlayerState.Falling;
            return StateKey;
        }
    }
}