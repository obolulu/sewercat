namespace _Project._Scripts.PlayerScripts
{
    public class PlayerLeapingState : BaseState<PlayerController.PlayerState>
    {
        private readonly PlayerController controller;
        private          bool             _isLeaping;

        public PlayerLeapingState(PlayerController.PlayerState key, PlayerController controller) : base(key)
        {
            this.controller = controller;
        }

        public override void EnterState()
        {
            _isLeaping = true;
        }

        public override void ExitState()
        {
            _isLeaping = false;
        }

        public override void UpdateState()
        {
            // The actual movement is handled by the weapon state
            controller.ApplyGravity(controller.groundedGravity);
        }

        public override PlayerController.PlayerState GetNextState()
        {
            if (!_isLeaping)
            {
                if (!controller.IsGrounded())
                    return PlayerController.PlayerState.Falling;
                
                return controller.HasMovementInput() 
                    ? PlayerController.PlayerState.Walking 
                    : PlayerController.PlayerState.Idle;
            }
            
            return StateKey;
        }
    }
}