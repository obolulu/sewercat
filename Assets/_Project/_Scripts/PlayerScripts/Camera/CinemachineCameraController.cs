using UnityEngine;
using Cinemachine;

public class CinemachineCameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    public float mouseSensitivity = 100f;
    
    [Header("Tilt Settings")]
    [SerializeField] private float tiltAngle = 5f;
    [SerializeField] private float tiltSpeed = 1f;
        
    [Header("Jump/Land Effects")]
    [SerializeField] private float jumpTiltAmount = 3f;
    [SerializeField] private float                    landTiltAmount = 2f;
    [SerializeField] private float                    recoverySpeed  = 10f;
    
    
    private                  CinemachineVirtualCamera virtualCamera;
    private                  CinemachinePOV           povExtension;
    private                  Transform                cameraTransform;
    
    void Start()
    {
        // Get references
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        povExtension = virtualCamera.GetCinemachineComponent<CinemachinePOV>();
        cameraTransform = virtualCamera.transform;
        
        // Setup initial cursor state
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        // Configure POV settings
        if (povExtension != null)
        {
            povExtension.m_VerticalAxis.m_MaxSpeed = mouseSensitivity;
            povExtension.m_HorizontalAxis.m_MaxSpeed = mouseSensitivity;
            
            // Set clamp values for vertical rotation
            povExtension.m_VerticalAxis.m_MinValue = -90f;
            povExtension.m_VerticalAxis.m_MaxValue = 90f;
        }
    }

    void Update()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            return;
        }

        // Apply camera tilt based on movement
        float targetTilt = InputManager.moveDirection.x * tiltAngle;
        float currentTilt = Mathf.LerpAngle(cameraTransform.localEulerAngles.z, targetTilt, tiltSpeed * Time.deltaTime);
        cameraTransform.localEulerAngles = new Vector3(
            cameraTransform.localEulerAngles.x,
            cameraTransform.localEulerAngles.y,
            currentTilt
        );
    }
}