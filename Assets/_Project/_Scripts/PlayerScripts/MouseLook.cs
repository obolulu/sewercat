using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
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
        playerBody.rotation = Quaternion.Euler(0, yRotation, 0f);//apply rotation to player
    }
}