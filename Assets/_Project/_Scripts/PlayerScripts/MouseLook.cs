using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    
    [SerializeField] private float tiltAngle = 10f; // Adjust the tilt angle as needed
    [SerializeField] private float tiltSpeed = 5f; // Adjust the tilt speed as needed
    
    float xRotation;
    float yRotation;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if(CursorLockMode.Locked != Cursor.lockState)
        {
            return;
        }
        //get mouse movement
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        //apply rotation to camera
        yRotation += mouseX;
        xRotation -= mouseY;//idk man
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);//prevent overrotation
        transform.rotation = Quaternion.Euler(xRotation,yRotation,0f);//apply rotation to camera

        float targetTilt = InputManager.moveDirection.x * tiltAngle;
        float currentTilt = Mathf.LerpAngle(transform.localEulerAngles.z, targetTilt, tiltSpeed * Time.deltaTime);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, currentTilt);
        
    }
}