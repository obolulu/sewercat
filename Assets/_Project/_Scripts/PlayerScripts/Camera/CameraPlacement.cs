using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlacement : MonoBehaviour
{
    //[SerializeField] private Transform cameraTransform;
    [SerializeField] private CharacterController characterController;
    void Update()
    {
        Vector3 characterWorldPosition = characterController.transform.position;
        Vector3 cameraOffset           = characterController.center;
        transform.position = characterWorldPosition + cameraOffset;
        
    }
}