using System;
using _Project._Scripts.PlayerScripts;
using UnityEngine;

public class CameraHeadbob : MonoBehaviour
{
    [Header("Walking Headbob")]
    [SerializeField] private float walkingBobbingSpeed = 14f;
    [SerializeField] private float walkingBobbingAmount           = 0.05f;
    [SerializeField] private float walkingHorizontalBobbingAmount = 0.03f;
    
    //ADD CROUCH LATER
    
    [Header("Breathing")]
    [SerializeField] private float breathingSpeed = 2f;
    [SerializeField] private float breathingAmount = 0.02f;
    
    [Header("Smoothing")]
    [SerializeField] private float smoothTransitionSpeed = 6f;

    [Header("References")]
    [SerializeField] private PlayerController playerController;

    private float _defaultPosY = 0;
    private float            _defaultPosX = 0;
    private float            _timer       = 0;
    private Vector3          targetBobPosition;
    
    private bool playNextFootstep = true;
    
    public static event Action<Vector3> OnFootstep;

    private void Start()
    {
        _defaultPosY = transform.localPosition.y;
        _defaultPosX = transform.localPosition.x;
    }
    
    private void Update()
    {
        if (!playerController.HeadbobEnabled) return;

        HandleHeadbob();
    }

    private void HandleHeadbob()
    {
        float speedPercent = playerController.CurrentMoveVelocity.magnitude / playerController.moveSpeed; // for crouch later

        if (playerController.HasMovementInput() && playerController.IsGrounded())
        {
            _timer += Time.deltaTime * walkingBobbingSpeed;

            float verticalBob = Mathf.Sin(_timer) * walkingBobbingAmount * speedPercent;
            verticalBob += Mathf.Sin(Time.time * breathingSpeed) * breathingAmount;
            
            float horizontalBob = Mathf.Cos(_timer/2f) * walkingHorizontalBobbingAmount * speedPercent;
            
            targetBobPosition = new Vector3(
                _defaultPosX + horizontalBob,
                _defaultPosY + verticalBob,
                transform.localPosition.z);
            // Play footstep sounds
            if (Mathf.Sin(_timer) < -0.99f && playNextFootstep) // Trigger at the peak
            {
                PlayFootstepSound();
                playNextFootstep = false;
            }
            else if (Mathf.Sin(_timer) > 0.99f) // Reset for the next cycle
            {
                playNextFootstep = true;
            }
        }
        else
        {
            _timer = 0f;
            float breathingBob = Mathf.Sin(Time.time * breathingSpeed) * breathingAmount;
            targetBobPosition = new Vector3(_defaultPosX,_defaultPosY+breathingBob,transform.localPosition.z);
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetBobPosition,
            Time.deltaTime * smoothTransitionSpeed);
    }

    private void PlayFootstepSound()
    {
        OnFootstep?.Invoke(playerController.transform.position);
    }
}
