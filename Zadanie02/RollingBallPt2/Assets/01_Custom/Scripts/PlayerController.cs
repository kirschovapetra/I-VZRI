using System;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
public class PlayerController : MonoBehaviour {
    public float speed;
    private Rigidbody rb;
    private Vector3 moveVec = Vector3.zero;
    private Vector3 actualDirection = Vector3.zero;

    protected bool jumping = false;	
    public Transform cameraPivot;
    public float jumpVelocity;
    public float distanceFromCamera;
    private float heading;
    public Transform cameraTransform;
    public GameManager gameManager;
    public AudioSource pickCoinAudio;
    
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
    }

    
    void FixedUpdate () {
        rb.AddForce(actualDirection*speed);
    }
    
    
    public void OnJump() {
        if (!jumping) {
            rb.AddForce(new Vector3(0, jumpVelocity, 0),ForceMode.Impulse); 
            jumping = true;
        }
    }

    public void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Ground"))
            jumping = false;
        if (other.gameObject.CompareTag("Hole"))
            gameManager.gameOver = true;
    }


    void OnMove(InputValue movementValue) {
  
        Vector2 movementVector = movementValue.Get<Vector2>();
        
        moveVec = new Vector3(movementVector.x, 0, movementVector.y);
        actualDirection = cameraTransform.TransformDirection(moveVec);
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag ("PickUp")) {
            pickCoinAudio.Play();
            other.gameObject.SetActive (false);
            gameManager.score++;
        }
        if (other.gameObject.CompareTag ("Finish")) {
            gameManager.finished = true;
        }
    }

}
