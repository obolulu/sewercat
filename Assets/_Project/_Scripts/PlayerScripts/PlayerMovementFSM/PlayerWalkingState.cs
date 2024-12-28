using UnityEngine;

namespace _Project._Scripts.PlayerScripts
{
    /*

    void Upate()
    {
        if (!Application.isPlaying) return; // TODO: find better fix
        if (vcam.Follow)
        {
            if (stage == CinemachineCore.Stage.Aim)
            {
                state.PositionCorrection.y += Mathf.Sin(Time.time * frequency) * amplitude;
                state.PositionCorrection.x += Mathf.Cos(Time.time * frequency/2) * amplitude*2;
            }
        }
    }
      
     */
    public class PlayerWalkingState : BaseState<PlayerController.PlayerState>
    {
        private readonly PlayerController controller;
        private          float            time;
        public PlayerWalkingState(PlayerController.PlayerState key, PlayerController controller) : base(key)
        {
            this.controller          =  controller;
        }

        public override void EnterState()
        {
            controller.ResetHorizontalVelocity();
            controller.ResetJump();
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

            if (controller.CheckJump())
            {
                return PlayerController.PlayerState.Jumping;
            }

            if (controller.CheckCrouch())
            {
                return PlayerController.PlayerState.Crouching;
            }

            if (!controller.HasMovementInput())
                return PlayerController.PlayerState.Idle;

            return StateKey;
        }


    }
}