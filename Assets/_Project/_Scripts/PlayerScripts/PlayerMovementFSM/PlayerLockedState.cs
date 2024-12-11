using UnityEngine;

namespace _Project._Scripts.PlayerScripts
{
    public class PlayerLockedState: BaseState<PlayerStateMachine.PlayerState>
    {
        private readonly PlayerController               controller;
        private          PlayerStateMachine.PlayerState previousState;
    
        public PlayerLockedState(PlayerStateMachine.PlayerState key, PlayerController controller) : base(key)
        {
            this.controller = controller;
        }
    
        public override void EnterState()
        {
            controller.LockPlayer();
            previousState = controller.IsGrounded() ? PlayerStateMachine.PlayerState.Idle : PlayerStateMachine.PlayerState.Falling;
        }
    
        public override void ExitState()
        {
            controller.UnlockPlayer();
        }
    
        public override void UpdateState()
        {
            controller.ApplyGravity(controller.gravity);
        }
    
        public override PlayerStateMachine.PlayerState GetNextState()
        {
            // Add your condition here to determine when to unlock
            // For example: if (someCondition) return previousState;
            return StateKey;
        }
    }
}