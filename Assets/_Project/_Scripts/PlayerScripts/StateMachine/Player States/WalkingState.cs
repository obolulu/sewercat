using UnityEngine;

namespace Scripts.Player_Scripts.Player_States
{
    public class WalkingState : BaseState<PlayerStateMachine.PlayerState>
    {
        
        private Transform cameraTransform;
        private PlayerStateMachine _playerStateMachine;
        public WalkingState(PlayerStateMachine.PlayerState key,PlayerStateMachine psm) : base(key)
        {
            _playerStateMachine = psm;
            cameraTransform = _playerStateMachine.cameraTransform;
        }

        
        
        public override void EnterState()
        {
            // PlayerStateMachine.rb.drag = PlayerStateMachine.groundDrag;
            UpdateState();
        }

        public override void ExitState()
        {
        }

        public override void UpdateState()
        {
            
            Movement();
            
            
            // ApplyDrag();
            
            // GetNextState();
        }

        public override PlayerStateMachine.PlayerState GetNextState()
        {
            if (_playerStateMachine.rb.velocity.magnitude < 0.1f)
            {
                return PlayerStateMachine.PlayerState.Idle;
            }

            if (!_playerStateMachine.isGrounded)
            {
                return PlayerStateMachine.PlayerState.Falling;
            }
            if(InputManager.StartJump && _playerStateMachine.isGrounded)
            {
                return PlayerStateMachine.PlayerState.Jumping;
            }

            return PlayerStateMachine.PlayerState.Walking;
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

        private void Movement()
        {
            
            Vector3 cameraForward = cameraTransform.forward;
            cameraForward.y = 0; // Ignore the vertical component
            cameraForward.Normalize();

            Vector3 cameraRight = cameraTransform.right;
            cameraRight.y = 0; // Ignore the vertical component
            cameraRight.Normalize();

            Vector3 targetDirection = (cameraForward * InputManager.moveDirection.y + cameraRight * InputManager.moveDirection.x).normalized;

            _playerStateMachine.rb.AddForce(targetDirection * _playerStateMachine.PlayerSpeed);
            
        }
    }
}