using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.InputSystem;

// ovladanie kamery
// zdroj: https://github.com/Yecats/GameDevTutorials/blob/master/tutorials/Unity/How-to-make-a-configurable-camera-with-the-new-Input-System/articles/pt-2-setting-up-the-input-system.md
public class CameraController : MonoBehaviour {
    public float moveSpeed;
    public float zoomSpeed;
    public float rotationSpeed = 2;
    public Transform lookAtTransform;
    public float zoom = 80;
    public float maxZoom = 120;
    public float minZoom = 5;
    
    private Vector2 movementVector;
    private Vector2 mouseRotation;
    private Vector3 offset;  
    private float scrollZ;
    private bool rightMouseButtonDown = false;
       
    void Awake() {
        offset = transform.position - lookAtTransform.position; 
    }
    
    
    void OnCameraZoom(InputValue scrollValue) {
        scrollZ = scrollValue.Get<Vector2>().y;
    }
    
    void OnCameraToggleRotation(InputValue inputValue) {
        rightMouseButtonDown = inputValue.Get<float>() == 1.0f;
    }
    
    void OnCameraRotation(InputValue rotationValue) {
        mouseRotation = rotationValue.Get<Vector2>().normalized;
    }
    
    void LateUpdate () {
        

        // rotacia
        float rotationX = rightMouseButtonDown ? mouseRotation.x : 0.0f;
        Quaternion camTurnAngle = Quaternion.AngleAxis(rotationX * rotationSpeed, Vector3.up);
        offset = camTurnAngle * offset;
        transform.position = lookAtTransform.position + offset;
        
        transform.LookAt(lookAtTransform);    
        
        // zoom
        zoom -= scrollZ * zoomSpeed;
        zoom = zoom > maxZoom ? maxZoom : zoom;
        zoom = zoom < minZoom ? minZoom : zoom;

        float newZoomValue = zoom - (lookAtTransform.position - transform.position).magnitude;
        Camera.main.fieldOfView = Mathf.Clamp(newZoomValue, 0, zoom);
    }
    
   
}