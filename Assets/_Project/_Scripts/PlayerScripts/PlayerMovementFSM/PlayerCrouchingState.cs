using UnityEngine;

namespace _Project._Scripts.PlayerScripts
{
    public class PlayerCrouchingState : BaseState<PlayerController.PlayerState>
    {
        private readonly PlayerController _controller;

        public PlayerCrouchingState(PlayerController.PlayerState key, PlayerController controller) : base(key)
        {
            _controller = controller;
        }

        public override void EnterState()
        {
            _controller.Crouch();
        }

        public override void ExitState()
        {
            Debug.Log("Exiting crouching state");
           _controller.UnCrouch();
        }

        public override void UpdateState()
        {
            _controller.Move(_controller.CrouchSpeedMultiplier);
        }

         public override PlayerController.PlayerState GetNextState()
        {
           if(!_controller.CheckCrouch())
               return _controller.HasMovementInput()
                   ? PlayerController.PlayerState.Walking
                   : PlayerController.PlayerState.Idle;
           //if (_controller.CheckJump())
               //return PlayerController.PlayerState.Jumping;
           if(!_controller.IsGrounded())
                return PlayerController.PlayerState.Falling;
           
            return StateKey;
        }
    }
}