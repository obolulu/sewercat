using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlacement : MonoBehaviour
{
    //[SerializeField] private Transform cameraTransform;
    [SerializeField] private CharacterController characterController;
    [SerializeField, Range(0.5f,2f)] private float cameraHeight = 1f;
    void Update()
    {
        Vector3 characterWorldPosition = characterController.transform.position;
        Vector3 cameraOffset           = characterController.center * cameraHeight;
        transform.position = characterWorldPosition + cameraOffset;
        
    }
}