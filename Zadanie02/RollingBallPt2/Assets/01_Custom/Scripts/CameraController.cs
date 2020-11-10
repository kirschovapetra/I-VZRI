using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


// https://www.youtube.com/watch?v=xcn7hz7J7sI&ab_channel=Jayanam
public class CameraController : MonoBehaviour {
    
    public Transform playerTransform;
    public Transform playerLookatTransform;
    
    public Vector3 offset;
    public float rotationSpeed = 5.0f;
    public Vector2 lookMovement = Vector2.zero;
    public Quaternion camTurnAngle;
    public float sensitivity = 1.0f;
    void Start () {
        offset = transform.position - playerLookatTransform.position;
    }
    
    
    public void OnLook(InputAction.CallbackContext context) {
        lookMovement = context.ReadValue<Vector2>().normalized * sensitivity; 

    }

    
    void LateUpdate () {
        camTurnAngle = Quaternion.AngleAxis(
                lookMovement.x * rotationSpeed, Vector3.up);
        
        offset = camTurnAngle * offset;

        transform.position = playerLookatTransform.position + offset;
        // transform.position = playerTransform.position + offset;

        transform.LookAt(playerLookatTransform);

    }
}