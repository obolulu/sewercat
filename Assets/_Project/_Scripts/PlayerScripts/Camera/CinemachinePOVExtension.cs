using System.Collections;
using System.Collections.Generic;
using _Project._Scripts.PlayerScripts;
using Cinemachine;
using Scripts.Player_Scripts.Player_States;
using UnityEngine;

public class CinemachinePOVExtension : CinemachineExtension
{
    [SerializeField] private float clampAngle = 80f;
    [SerializeField] private float verticalSensitivity = 10f;
    [SerializeField] private float horizontalSensitivity = 10f;

    [Header("Movement Tilt")] 
    [SerializeField] private bool isTiltEnabled;
    [SerializeField] private float tiltAngle = 5f;
    [SerializeField] private float tiltSpeed = 8f;

    [Header("Jump/Land Effects")] 
    [SerializeField] private bool isJumpTiltEnabled;
    [SerializeField] private float jumpTiltAmount   = 3f;
    [SerializeField] private float landTiltAmount   = 2f;
    [SerializeField] private float recoverySpeed    = 10f;  
    [SerializeField] private float groundCheckDelay = 0.1f; // seconds
    
    [Header("References")]
    [SerializeField] private PlayerController controller;
    

    private Vector2 currentRotation;
    private float   timeSinceJump;
    private float   currentTilt;
    private float   verticalTilt;
    private float   targetVerticalTilt;
    private bool    wasGrounded;
    
    protected override void Awake()
    {
        Cursor.lockState       =  CursorLockMode.Locked;
        base.Awake();
        currentRotation = Vector2.zero;
        wasGrounded     = controller.IsGrounded();
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam,  CinemachineCore.Stage stage,
                                                      ref CameraState              state, float deltaTime)
    {
        if (!Application.isPlaying) return; // TODO: find better fix
        if (!vcam.Follow) return;
        if(Cursor.lockState == CursorLockMode.None) return;
        if (stage == CinemachineCore.Stage.Aim)
        {
            Vector2 deltaInput = InputManager.GetMouseDelta();
            currentRotation.x += deltaInput.x * verticalSensitivity   * Time.deltaTime;
            currentRotation.y += deltaInput.y * horizontalSensitivity * Time.deltaTime;
            currentRotation.y =  Mathf.Clamp(currentRotation.y, -clampAngle, clampAngle);
            
            if(isTiltEnabled) HandleMovementTilt(deltaTime);
            if(isJumpTiltEnabled) HandleJumpLandTilt(deltaTime);

            Quaternion rotation     = Quaternion.Euler(-currentRotation.y + verticalTilt, currentRotation.x , 0);
            Quaternion tiltRotation = Quaternion.AngleAxis(currentTilt, Vector3.forward);


            state.RawOrientation =  rotation * tiltRotation;

        }
    }

    private void HandleMovementTilt(float deltaTime)
    {
        float targetTilt = InputManager.moveDirection.x * tiltAngle;
        currentTilt = Mathf.Lerp(currentTilt, targetTilt, tiltSpeed * deltaTime);
    }
    private void HandleJumpLandTilt(float deltaTime)
    {
        if (controller.CurrentState.StateKey == PlayerController.PlayerState.Jumping)
        {
            timeSinceJump      = 0f;
            targetVerticalTilt = -jumpTiltAmount;
        }
        
        if (!wasGrounded && controller.IsGrounded() && timeSinceJump >= groundCheckDelay)
        {
            targetVerticalTilt = -landTiltAmount;
        }

        timeSinceJump += deltaTime;
        verticalTilt  =  Mathf.Lerp(verticalTilt, targetVerticalTilt, deltaTime * recoverySpeed);
        wasGrounded   =  controller.IsGrounded();
    }
}