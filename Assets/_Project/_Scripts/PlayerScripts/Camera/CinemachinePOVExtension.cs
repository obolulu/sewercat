using System.Collections;
using System.Collections.Generic;
using _Project._Scripts.PlayerScripts;
using Cinemachine;
using UnityEngine;

public class CinemachinePOVExtension : CinemachineExtension
{
    [SerializeField] private float clampAngle = 80f;
    [SerializeField] private float verticalSensitivity = 10f;
    [SerializeField] private float horizontalSensitivity = 10f;
    
    [Header("Movement Tilt")]
    [SerializeField] private float tiltAngle = 5f;
    [SerializeField] private float tiltSpeed = 8f;
    
    [Header("Jump/Land Effects")]
    [SerializeField] private float jumpTiltAmount = 3f;
    [SerializeField] private float              landTiltAmount = 2f;
    [SerializeField] private float              recoverySpeed  = 10f;  
    
    [Header("References")]
    [SerializeField] private PlayerController controller;
    

    
    private Vector3 startingRotation;
    private float   currentTilt;
    private float   verticalTilt;
    private bool    wasGrounded;
    protected override void Awake()
    {
        Cursor.lockState       =  CursorLockMode.Locked;
        base.Awake();
        startingRotation = transform.localRotation.eulerAngles;
        wasGrounded      = controller.IsGrounded();
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam,  CinemachineCore.Stage stage,
                                                      ref CameraState              state, float deltaTime)
    {
        if (!Application.isPlaying) return; // TODO: find better fix
        if (!vcam.Follow) return;
        if (stage == CinemachineCore.Stage.Aim)
        {
            Vector2 deltaInput = InputManager.GetMouseDelta();
            startingRotation.x   += deltaInput.x * verticalSensitivity   * Time.deltaTime;
            startingRotation.y   += deltaInput.y * horizontalSensitivity * Time.deltaTime;
            startingRotation.y   =  Mathf.Clamp(startingRotation.y, -clampAngle, clampAngle);
            
            float targetTilt = InputManager.moveDirection.x * tiltAngle;
            currentTilt = Mathf.Lerp(currentTilt, targetTilt, tiltSpeed * deltaTime);
            
            HandleJumpLandTilt(deltaTime);

            state.RawOrientation = Quaternion.Euler(-startingRotation.y + verticalTilt, startingRotation.x, currentTilt);

        }
    }

    private void HandleJumpLandTilt(float deltaTime)
    {
        if (controller.CurrentState.StateKey == PlayerController.PlayerState.Jumping)
        {
            verticalTilt = -jumpTiltAmount;
        }
        
        if (!wasGrounded && controller.IsGrounded())
        {
            verticalTilt = landTiltAmount;
        }
        
        verticalTilt = Mathf.Lerp(verticalTilt, 0, deltaTime * recoverySpeed);
        wasGrounded  = controller.IsGrounded();
    }
}