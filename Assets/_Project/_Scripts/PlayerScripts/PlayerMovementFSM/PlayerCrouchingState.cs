namespace _Project._Scripts.PlayerScripts
{
    public class PlayerCrouchingState: BaseState<PlayerController.PlayerState>
    {
        private PlayerController controller;
        
        public PlayerCrouchingState(PlayerController.PlayerState key, PlayerController controller) : base(key)
        {
            this.controller = controller;
        }

        public override void EnterState()
        {
            throw new System.NotImplementedException();
        }

        public override void ExitState()
        {
            throw new System.NotImplementedException();
        }

        public override void UpdateState()
        {
            throw new System.NotImplementedException();
        }

        public override PlayerController.PlayerState GetNextState()
        {
            throw new System.NotImplementedException();
        }
    }
}