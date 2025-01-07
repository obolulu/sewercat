using UnityEngine;
using _Project._Scripts.PlayerScripts.Weapons.Claws.States.StateDatas;
using _Project._Scripts.ScriptBases;

namespace _Project._Scripts.PlayerScripts.Weapons.Claws.States
{
    public class LeapingClawState : BaseState<ClawsWeaponFSM.ClawsWeaponState>
    {
        private readonly ClawsWeaponFSM _weaponFSM;
        private readonly PlayerController _playerController;
        private float _leapStartTime;
        private Vector3 _startPosition;
        private Vector3 _initialLeapVelocity;
        private float _maxHorizontalSpeed;
        private float _initialVerticalVelocity;
        private float _currentLeapTime;

        private ClawsWeaponFSM.ClawsWeaponState nextState;
        private ClawLeapStateData data;

        public LeapingClawState(ClawsWeaponFSM.ClawsWeaponState key, ClawsWeaponFSM weaponFSM,
                                ClawLeapStateData data, PlayerController playerController) 
            : base(key)
        {
            _weaponFSM = weaponFSM;
            _playerController = playerController;
            this.data = data;
        }

        public override void EnterState()
        {
            _currentLeapTime = 0;
            _weaponFSM.PlayerController.SetLeaping(true);
            _leapStartTime = Time.time;
            _startPosition = _weaponFSM.transform.position;
            nextState = ClawsWeaponFSM.ClawsWeaponState.Leaping;
            
            CalculateLeapVelocity();
            _weaponFSM.PlayerController.TransitionToState(PlayerController.PlayerState.Leaping);
        }

        private void CalculateLeapVelocity()
        {
            Vector3 forward = _weaponFSM.MainCamera.transform.forward;
            forward.Normalize();

            // Calculate initial velocity for the leap
            _initialLeapVelocity = forward * data.leapDistance / data.leapDuration;
            _initialLeapVelocity.y = (data.leapHeight * 2f) / data.leapDuration; // Initial upward velocity
            
            // Apply the velocity to the player controller
            Vector3 horizontalVelocity = new Vector3(_initialLeapVelocity.x, 0, _initialLeapVelocity.z);
            
            _maxHorizontalSpeed = horizontalVelocity.magnitude;
            _weaponFSM.PlayerController.verticalVelocity = _initialLeapVelocity;
        }

        private void UpdateLeapDirection()
        {
            // Get the camera's forward direction (including vertical rotation)
            Vector3 cameraForward = _weaponFSM.MainCamera.transform.forward;
            
            // Calculate the vertical velocity with gravity
            float currentVerticalVelocity = _initialVerticalVelocity - (Physics.gravity.y * _currentLeapTime);
            
            // Calculate horizontal direction from camera
            Vector3 horizontalVelocity = cameraForward * _maxHorizontalSpeed;
            
            // Combine horizontal and vertical velocities
            Vector3 finalVelocity = new Vector3(
                horizontalVelocity.x,
                currentVerticalVelocity,
                horizontalVelocity.z
            );

            _weaponFSM.PlayerController.verticalVelocity = finalVelocity;
            _weaponFSM.PlayerController.moveVelocity = finalVelocity;
        }
        
        public override void UpdateState()
        {
            if (!_weaponFSM.PlayerController.IsLeaping)
            {
                CompleteLeap();
                return;
            }

            float elapsedTime = Time.time - _leapStartTime;
            _weaponFSM.PlayerController.Move(data.airControl);
            
            Vector3 currentHorizontalVelocity = new Vector3(
                _weaponFSM.PlayerController.moveVelocity.x,
                0,
                _weaponFSM.PlayerController.moveVelocity.z
            );
            if (currentHorizontalVelocity.magnitude > _maxHorizontalSpeed)
            {
                // Normalize and scale to max speed while preserving direction
                currentHorizontalVelocity = currentHorizontalVelocity.normalized * _maxHorizontalSpeed;
                
                // Reconstruct full velocity with clamped horizontal components
                _weaponFSM.PlayerController.moveVelocity = new Vector3(
                    currentHorizontalVelocity.x,
                    _weaponFSM.PlayerController.moveVelocity.y,
                    currentHorizontalVelocity.z
                );
            }
            
            // Check if leap should end
            if ((elapsedTime >= data.leapDuration && _weaponFSM.PlayerController.IsGrounded())
                || _weaponFSM.PlayerController.IsFacingObstacle() )
            {
                CompleteLeap();
            }
            if(_weaponFSM.PlayerController.moveVelocity.magnitude > _initialLeapVelocity.magnitude)
                _weaponFSM.PlayerController.moveVelocity = _initialLeapVelocity;
        }

        private void CompleteLeap()
        {
            _weaponFSM.PlayerController.SetLeaping(false);
            PerformAttack();
            nextState = ClawsWeaponFSM.ClawsWeaponState.Default;
        }

        private void PerformAttack()
        {
            _weaponFSM.AttackFeedbacks?.PlayFeedbacks();
            HitDetect();
        }

        private void HitDetect()
        {
            Collider[] hitColliders = Physics.OverlapSphere(_weaponFSM.transform.position, 
                                                          data.attackRange, 
                                                          data.whatIsDamageable);

            foreach (Collider hit in hitColliders)
            {
                if (hit.TryGetComponent<IDamageable>(out var enemy))
                {
                    Vector3 hitDirection = (hit.transform.position - _weaponFSM.transform.position).normalized;
                    _weaponFSM.HitFeedbacks?.PlayFeedbacks();
                    enemy.TakeDamage(data.attackDamage, hitDirection);
                }
            }
        }

        public override void ExitState()
        {
            _weaponFSM.PlayerController.SetLeaping(false);
            _weaponFSM.PlayerController.TransitionToState(PlayerController.PlayerState.Walking);
            _weaponFSM.ResetAnimation();
        }

        public override ClawsWeaponFSM.ClawsWeaponState GetNextState()
        {
            return nextState;
        }
    }
}