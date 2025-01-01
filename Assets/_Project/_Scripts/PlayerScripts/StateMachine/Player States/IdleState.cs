using UnityEngine;

namespace Scripts.Player_Scripts.Player_States
{
    public class IdleState : BaseState<PlayerStateMachine.PlayerState>
    {
        private Vector2 _moveDirection;
        private Vector3 _velocity;
        private GameObject _playerCharacter;
        private InputManager _inputManager;
        private PlayerStateMachine _playerStateMachine;
        private Rigidbody _rigidbody;
        
        public IdleState(PlayerStateMachine.PlayerState key, PlayerStateMachine psm) : base(key)
        {
            _playerStateMachine = psm;
        }

        public override void EnterState()
        {

        }

        public override void ExitState()
        {

        }

        public override void UpdateState()
        {
            //GetNextState();
        }

        public override PlayerStateMachine.PlayerState GetNextState()
        {

            
            if(InputManager.State.IsJumping && _playerStateMachine.isGrounded)
            {
                return PlayerStateMachine.PlayerState.Jumping;
            }
            if (!_playerStateMachine.isGrounded)
            {
                return PlayerStateMachine.PlayerState.Falling;
            }
            if(InputManager.State.MoveDirection.magnitude > 0.1f)
            {
                return PlayerStateMachine.PlayerState.Walking;
            }
            return PlayerStateMachine.PlayerState.Idle;
        }

        public override void OnTriggerEnter(Collider other)
        {
            throw new System.NotImplementedException();
        }

        public override void OnTriggerStay(Collider other)
        {
            throw new System.NotImplementedException();
        }

        public override void OnTriggerExit(Collider other)
        {
            throw new System.NotImplementedException();
        }
    }
}




