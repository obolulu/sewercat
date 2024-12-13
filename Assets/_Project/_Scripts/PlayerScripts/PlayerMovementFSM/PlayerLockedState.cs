using UnityEngine;

namespace _Project._Scripts.PlayerScripts
{
    public class PlayerLockedState: BaseState<PlayerController.PlayerState>
    {
        private readonly PlayerController               controller;
        private          PlayerStateMachine.PlayerState previousState;
    
        public PlayerLockedState(PlayerController.PlayerState key, PlayerController controller) : base(key)
        {
            this.controller = controller;
        }
    
        public override void EnterState()
        {
            controller.LockPlayer();
            previousState = controller.IsGrounded() ? PlayerStateMachine.PlayerState.Idle : PlayerStateMachine.PlayerState.Falling;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    
        public override void ExitState()
        {
            controller.UnlockPlayer();
            Cursor.lockState = CursorLockMode.Locked;
        }
    
        public override void UpdateState()
        {
            //controller.ApplyGravity(controller.gravity);
        }
    
        public override PlayerController.PlayerState GetNextState()
        {
            // Add your condition here to determine when to unlock
            // For example: if (someCondition) return previousState;
            return StateKey;
        }
    }
}