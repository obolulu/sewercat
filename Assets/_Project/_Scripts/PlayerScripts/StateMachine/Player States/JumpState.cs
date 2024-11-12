using UnityEngine;

namespace Scripts.Player_Scripts.Player_States
{
    public class JumpState : BaseState<PlayerStateMachine.PlayerState>//why???
    {
        private Vector3 _velocity;
        private PlayerStateMachine _playerStateMachine;

        public JumpState(PlayerStateMachine.PlayerState key, PlayerStateMachine psm) : base(key)
        {
            _playerStateMachine = psm;
        }

        public override void EnterState()
        {
            _playerStateMachine.rb.velocity = new Vector3(_playerStateMachine.rb.velocity.x * 1.1f,_playerStateMachine.rb.velocity.y, _playerStateMachine.rb.velocity.z * 1.1f);
            GetNextState();
            _playerStateMachine.GetComponent<ConstantForce>().force /=3;
        }

        public override void ExitState()
        {
            _playerStateMachine.GetComponent<ConstantForce>().force *=3;
        }

        public override void UpdateState()
        {

            if (_playerStateMachine.isGrounded)
            {
                Jump();
            }
            
            JumpMovement();


        }
        private void Jump()
        {
            
            if (_playerStateMachine.jumpTimer >= _playerStateMachine.jumpCooldown)
            {
                _velocity.y = _playerStateMachine.playerJumpForce;
                _playerStateMachine.rb.AddForce(new Vector3(0,_playerStateMachine.playerJumpForce,0), ForceMode.Impulse);
                _playerStateMachine.jumpTimer = 0;
            }

             
        }
        
        public void JumpMovement()
        {
            Vector3 cameraForward = _playerStateMachine.cameraTransform.forward;
            cameraForward.y = 0; // Ignore the vertical component
            cameraForward.Normalize();

            Vector3 cameraRight = _playerStateMachine.cameraTransform.right;
            cameraRight.y = 0; // Ignore the vertical component
            cameraRight.Normalize();

            Vector3 targetDirection = (cameraForward * InputManager.moveDirection.y + cameraRight * InputManager.moveDirection.x).normalized;

            _playerStateMachine.rb.AddForce(targetDirection * _playerStateMachine.PlayerSpeed/4);            
        }

        public override PlayerStateMachine.PlayerState GetNextState()
        {
            if (!_playerStateMachine.isGrounded && _playerStateMachine.rb.velocity.y < 0)
            {
                return PlayerStateMachine.PlayerState.Falling;
            }
            
            if (InputManager.StartJump)
            {
                return PlayerStateMachine.PlayerState.Jumping;
            }
            
            if (_playerStateMachine.isGrounded)
            {
                return PlayerStateMachine.PlayerState.Walking;
            }
            return PlayerStateMachine.PlayerState.Jumping;
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