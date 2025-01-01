using UnityEngine;

public class FallState : BaseState<PlayerStateMachine.PlayerState>
{
    // get rid of the transform in here, change how the movement is reliant on the camera direction
     private Transform cameraTransform;
     private PlayerStateMachine _playerStateMachine;

     public FallState(PlayerStateMachine.PlayerState key, PlayerStateMachine psm) : base(key)
    {
        _playerStateMachine = psm;
        cameraTransform = Camera.main?.transform;
    }

    public override void EnterState()
    {
        // PlayerStateMachine.rb.drag = PlayerStateMachine.airDrag;

    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        FallCharacter();
        //GetNextState();
    }

    public override PlayerStateMachine.PlayerState GetNextState()
    {
        if (_playerStateMachine.isGrounded)
        {
            return PlayerStateMachine.PlayerState.Walking;
        }
        return PlayerStateMachine.PlayerState.Falling;
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
    
    public void FallCharacter()
         {

            Vector3 cameraForward = cameraTransform.forward;
            cameraForward.y = 0; // Ignore the vertical component
            cameraForward.Normalize();

            Vector3 cameraRight = cameraTransform.right;
            cameraRight.y = 0; // Ignore the vertical component
            cameraRight.Normalize();

            Vector3 targetDirection = (cameraForward * InputManager.State.MoveDirection.y + cameraRight
                * InputManager.State.MoveDirection.x).normalized;

            _playerStateMachine.rb.AddForce(targetDirection * _playerStateMachine.PlayerSpeed/4);            
        }
}
