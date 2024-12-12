using _Project._Scripts.PlayerScripts;
using UnityEngine;

public class CameraTiltSystem : MonoBehaviour
{
    [Header("Movement Tilt")]
    [SerializeField] private float maxTiltAngle = 3f;
    [SerializeField] private float tiltSpeed = 8f;
    
    [Header("Jump/Land Effects")]
    [SerializeField] private float jumpTiltAmount = 5f;
    [SerializeField] private float landTiltAmount = 3f;
    [SerializeField] private float recoverySpeed  = 10f;
    
    [Header("references")]
    [SerializeField] private PlayerController controller;
    
    
    private float              currentTilt;
    private float              targetTilt;
    private float              verticalTilt;
    private bool               wasGrounded;
    

    private void Start()
    {
        wasGrounded = controller.IsGrounded();
    }

    private void Update()
    {
        HandleMovementTilt();
        HandleJumpLandTilt();
        ApplyTilt();
    }

    private void HandleMovementTilt()
    {
        float inputX = InputManager.moveDirection.x;
        targetTilt = inputX * maxTiltAngle;
    }

    private void HandleJumpLandTilt()
    {
        if (controller.currentPlayerState.StateKey == PlayerController.PlayerState.Jumping)
        {
            verticalTilt = -jumpTiltAmount;
        }
        
        if (!wasGrounded && controller.IsGrounded())
        {
            verticalTilt = landTiltAmount;
        }
        
        verticalTilt = Mathf.Lerp(verticalTilt, 0, Time.deltaTime * recoverySpeed);
        wasGrounded  = controller.IsGrounded();
    }

    private void ApplyTilt()
    {
        currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.deltaTime * tiltSpeed);
        
        Vector3 finalRotation = new Vector3(verticalTilt, 0, -currentTilt);
        transform.localRotation = Quaternion.Euler(finalRotation);
    }
}