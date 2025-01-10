using System;
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
    [SerializeField/*, Range(0f,1f)*/] private float slowedSensitivityMultiplier = 2f;

    [Header("Movement Tilt")] 
    [SerializeField] private bool isTiltEnabled;
    [SerializeField] private float tiltAngle = 5f;
    [SerializeField] private float tiltSpeed = 8f;

    [Header("Jump/Land Effects")] 
    [SerializeField] private bool isJumpTiltEnabled;
    [SerializeField] private float jumpTiltAmount = 3f;
    [SerializeField] private float fallTiltAmount = 0f;
    [SerializeField] private float landTiltAmount   = 2f;
    [SerializeField] private float recoverySpeed    = 10f;  

    [Header("References")]
    [SerializeField] private PlayerController controller;

    public static event Action<Vector3> OnFootstep; // Event for footstep sounds

    private Vector2 currentRotation;
    private float   timeSinceJump;
    private float   currentTilt;
    private float   verticalTilt;
    private float   targetVerticalTilt;
    private bool    wasGrounded;

    private float _timer;
    private bool playNextFootstep = true;

    protected override void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        base.Awake();
        currentRotation = Vector2.zero;
        wasGrounded = controller.IsGrounded();
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage,
                                                      ref CameraState state, float deltaTime)
    {
        if (!Application.isPlaying) return; // TODO: find better fix
        if (!vcam.Follow) return;
        if (Cursor.lockState == CursorLockMode.None) return;
        if (stage == CinemachineCore.Stage.Aim)
        {
            Vector2 deltaInput = InputManager.State.MouseDelta;
            if (TimeManager.TimeScale < 1f) deltaInput *= TimeManager.TimeScale * slowedSensitivityMultiplier;
            currentRotation.x += deltaInput.x * verticalSensitivity * Time.unscaledDeltaTime;
            currentRotation.y += deltaInput.y * horizontalSensitivity * Time.unscaledDeltaTime;
            currentRotation.y = Mathf.Clamp(currentRotation.y, -clampAngle, clampAngle);

            if (isTiltEnabled) HandleMovementTilt(deltaTime);
            if (isJumpTiltEnabled) HandleJumpLandTilt(deltaTime);
            HandleFootsteps(deltaTime); // Call the footstep handling method

            Quaternion rotation = Quaternion.Euler(-currentRotation.y + verticalTilt, currentRotation.x, 0);
            state.RawOrientation = rotation;
        }
    }

    private void HandleMovementTilt(float deltaTime)
    {
        float targetTilt = InputManager.State.MoveDirection.x * tiltAngle;
        currentTilt = Mathf.Lerp(currentTilt, targetTilt, tiltSpeed * deltaTime);
    }

    private void HandleJumpLandTilt(float deltaTime)
    {
        if (controller.CurrentState.StateKey == PlayerController.PlayerState.Jumping)
        {
            timeSinceJump = 0f;
            targetVerticalTilt = -jumpTiltAmount;
        }
        else if (controller.CurrentState.StateKey == PlayerController.PlayerState.Falling)
        {
            targetVerticalTilt = -fallTiltAmount;
        }
        else
        {
            targetVerticalTilt = -landTiltAmount;
        }

        timeSinceJump += deltaTime;
        verticalTilt = Mathf.Lerp(verticalTilt, targetVerticalTilt, deltaTime * recoverySpeed);
        wasGrounded = controller.IsGrounded();
    }

    private void HandleFootsteps(float deltaTime)
    {
        float speedPercent = controller.CurrentMoveVelocity.magnitude / controller.moveSpeed;

        if (controller.HasMovementInput() && controller.IsGrounded())
        {
            _timer += deltaTime * 14f; // Walking bobbing speed

            // Trigger footstep sound at the peak of the bob
            if (Mathf.Sin(_timer) < -0.99f && playNextFootstep)
            {
                OnFootstep?.Invoke(controller.transform.position); // Invoke footstep event
                playNextFootstep = false;
            }
            else if (Mathf.Sin(_timer) > 0.99f) // Reset for the next cycle
            {
                playNextFootstep = true;
            }
        }
        else
        {
            _timer = 0f; // Reset timer if not moving
        }
    }
}