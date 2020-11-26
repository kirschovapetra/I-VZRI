using UnityEngine;
using UnityEngine.InputSystem;

// ovladanie kamery
// zdroj: https://www.youtube.com/watch?v=xcn7hz7J7sI&ab_channel=Jayanam
public class CameraController : MonoBehaviour {
    
    public Transform playerLookatTransform;        // bod nad playerom, kde sa pozera kamera
    public Vector3 offset;                         // offset kamery od playera
    public float rotationSpeed = 2f;               // rychlost otacania kamery
    private Vector2 lookMovement = Vector2.zero;    // suradnice otocenia mysou

    void Start() {
        offset = transform.position - playerLookatTransform.position; 
    }
    
    public void OnLook(InputAction.CallbackContext context) {
        lookMovement = context.ReadValue<Vector2>().normalized; // ziskanie suradnic z otocenia mysou
    }
    
    void LateUpdate () {
        // uhol otocenia kamery
        Quaternion camTurnAngle = Quaternion.AngleAxis(lookMovement.x * rotationSpeed, Vector3.up);
        // update offsetu
        offset = camTurnAngle * offset;
        // update polohz kamery
        transform.position = playerLookatTransform.position + offset;
        // nastavenie, aby sa kamera pozerala na playera
        transform.LookAt(playerLookatTransform);

    }
}