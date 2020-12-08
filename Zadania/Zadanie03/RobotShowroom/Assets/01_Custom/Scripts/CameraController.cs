using UnityEngine;
using UnityEngine.InputSystem;

// ovladanie kamery
public class CameraController : MonoBehaviour {
    // objekt, na ktory sa pozera kamera (robot)
    public Transform lookAtTransform;
    
    // zoom
    public float zoomSpeed;
    private float scrollZ;
    private float zoom = 80;
    private float maxZoom = 120;
    private float minZoom = 5;

    // rotacia
    public bool rotate = false;
    public float rotationSpeed = 2;
    public float automaticRotationSpeed = 40;
    private Vector2 mouseRotation;
    private Vector3 offset;
    private bool rightMouseButtonDown = false;
    private float maxY = 2.5f;
    private float minY = -1;
   
    void Start() {
        offset = transform.position - lookAtTransform.position;
    }
    
    // Input System - zoom
    void OnCameraZoom(InputValue scrollValue) {
        scrollZ = scrollValue.Get<Vector2>().y;
    }
    
    // Input System - zapnutie rotacie
    void OnCameraToggleRotation(InputValue inputValue) {
        rightMouseButtonDown = inputValue.Get<float>() == 1.0f;
    }
    
    // Input System - rotacia
    void OnCameraRotation(InputValue rotationValue) {
        mouseRotation = rotationValue.Get<Vector2>().normalized;
    }

    void LateUpdate () {
        // automaticka rotacia okolo robota
        if (rotate) {
            transform.RotateAround(
                lookAtTransform.position, 
                Vector3.up, 
                automaticRotationSpeed * Time.deltaTime);
        }
        // manualna rotacia
        else {

            // if (transform.eulerAngles.x >= maxRotation || transform.eulerAngles.x <= minRotation) 
            //     return;
            
            // suradnice rotacie
            float rotationX = rightMouseButtonDown? mouseRotation.x : 0.0f;
            float rotationY = rightMouseButtonDown? mouseRotation.y : 0.0f;

      

            Quaternion camTurnAngleX = Quaternion.AngleAxis(rotationX * rotationSpeed, Vector3.up);
            Quaternion camTurnAngleY = Quaternion.AngleAxis(rotationY * rotationSpeed/2, Vector3.left);
            // vypocitanie noveho offsetu
            offset = camTurnAngleX * camTurnAngleY * offset;

            // nastavenie novej polohy kamery
            Vector3 newPosition = lookAtTransform.position + offset;
            if (newPosition.y >= minY && newPosition.y <= maxY) {
                transform.position = newPosition;
                // kamera sa pozera na robota
                transform.LookAt(lookAtTransform);
            }

            // zoom
            zoom -= scrollZ * zoomSpeed;
            // od minZoom po maxZoom
            zoom = zoom > maxZoom ? maxZoom : zoom;
            zoom = zoom < minZoom ? minZoom : zoom;
    
            // field of view kamery
            float newZoomValue = zoom - (lookAtTransform.position - transform.position).magnitude;
            Camera.main.fieldOfView = Mathf.Clamp(newZoomValue, 0, zoom);
        }
    }
}