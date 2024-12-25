using _Project._Scripts.PlayerScripts.Weapons.Claws.States.StateDatas;
using UnityEngine;

namespace _Project._Scripts.PlayerScripts.Weapons.Claws.States
{
    public class LeapingClawState : BaseState<ClawsWeaponFSM.ClawsWeaponState>
    {
        private readonly ClawsWeaponFSM _weaponFSM;
        private bool _isLeaping;
        private float _leapStartTime;
        private Vector3 _startPosition;
        private Vector3 _targetPosition;
        
        // Configurable parameters
        private          ClawsWeaponFSM.ClawsWeaponState nextState;
        private          ClawLeapStateData data; 
        public LeapingClawState(ClawsWeaponFSM.ClawsWeaponState key, ClawsWeaponFSM weaponFSM, ClawLeapStateData data) 
            : base(key)
        {
            _weaponFSM = weaponFSM;
            this.data  = data;
        }

        public override void EnterState()
        {
            _isLeaping = true;
            _leapStartTime = Time.time;
            _startPosition = _weaponFSM.transform.position;
            nextState = ClawsWeaponFSM.ClawsWeaponState.Leaping;
            CalculateLeapTarget();
            _weaponFSM.PlayerController.TransitionToState(PlayerController.PlayerState.Leaping);
        }

        public override void UpdateState()
        {
            if (!_isLeaping) return;

            float elapsedTime  = Time.time - _leapStartTime;
            float leapProgress = elapsedTime / data.leapDuration;

            // Remove the progress clamp to allow movement beyond target
            // Calculate the base motion vector
            Vector3 moveDirection   = (_targetPosition - _startPosition).normalized;
            float   totalDistance   = data.leapDistance * (leapProgress);
            Vector3 currentPosition = _startPosition + moveDirection * totalDistance;

            // Calculate height using a modified arc that peaks at the middle and descends
            float heightProgress;
            if (leapProgress <= 0.5f)
            {
                // Rising phase - use first half of sine wave
                heightProgress = leapProgress * 2f; // Scale to 0-1 range for first half
                float heightOffset = data.leapHeight * Mathf.Sin(heightProgress * Mathf.PI * 0.5f);
                currentPosition.y = _startPosition.y + heightOffset;
            }
            else
            {
                // Falling phase - accelerating downward
                float fallProgress = (leapProgress - 0.5f) * 2f; // Scale to 0-1 range for second half
                float heightOffset = data.leapHeight       * (1 - fallProgress * fallProgress); // Quadratic fall
                currentPosition.y = _startPosition.y + heightOffset;
            }

            // Update position
            _weaponFSM.PlayerController.transform.position = currentPosition;

            // Only complete the leap when grounded and past the initial target
            if ((leapProgress > 1f && _weaponFSM.PlayerController.IsGrounded())
                || _weaponFSM.PlayerController.IsFacingObstacle())
            {
                CompleteLeap();
            }
        }
        
        private void CalculateLeapTarget()
        {
            Vector3 forward = _weaponFSM.MainCamera.transform.forward;
            forward.y = 0;
            forward.Normalize();

            // For now, just leap forward
            _targetPosition = _weaponFSM.transform.position + forward * data.leapDistance;

            /*
            // Face the leap direction
            if (forward != Vector3.zero)
            {
                _weaponFSM.transform.rotation = Quaternion.LookRotation(forward);
            }*/
        }

        private void CompleteLeap()
        {
            _isLeaping = false;
            PerformAttack();
            nextState = ClawsWeaponFSM.ClawsWeaponState.Default;
        }

        private void PerformAttack()
        {
            // Trigger attack feedback
            _weaponFSM.AttackFeedbacks?.PlayFeedbacks();
            
            // TODO: Implement actual attack logic here
        }

        public override void ExitState()
        {
            _isLeaping = false;
            _weaponFSM.PlayerController.TransitionToState(PlayerController.PlayerState.Walking);
            _weaponFSM.ResetState();
        }

        public override ClawsWeaponFSM.ClawsWeaponState GetNextState()
        {
            return nextState;
        }
    }
}