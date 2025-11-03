using System;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float parallaxSpeed;
    private Vector3 _previousCameraPosition;

    private void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
        
        _previousCameraPosition = cameraTransform.position;
    }

    private void FixedUpdate()
    {
        Vector3 cameraPosition = cameraTransform.position;
        Vector3 cameraDelta = cameraPosition - _previousCameraPosition;
        
        transform.position += cameraDelta * parallaxSpeed;
        
        _previousCameraPosition = cameraTransform.position;
    }
}
