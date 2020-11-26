using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    // kamera
    public Transform cameraTransform;
    
    // referencovane scripty
    public GameManager gameManager;
    public UIManager uiManager;
    
    // audio
    public AudioSource pickCoinAudio;
    public AudioSource hitAudio;
    public AudioSource finishAudio;
    public AudioSource particleAudio;
    
    // rychlost pohybu
    public float speed;
    // skakanie
    public bool jumping;
    public float jumpSpeed;
    
    private Rigidbody rb;
    private Vector3 moveVector = Vector3.zero;
    private GameObject[] coins;
    
   
 
    void Start() {
        rb = GetComponent<Rigidbody>();
        coins = GameObject.FindGameObjectsWithTag("Treasure");
    }

    void FixedUpdate () { rb.AddForce(moveVector*speed); }
    
    public void OnJump() {
        if (!jumping) {
            rb.AddForce(new Vector3(0, jumpSpeed, 0), ForceMode.Impulse); 
            jumping = true;
        }
    }

    public void OnCollisionEnter(Collision other) {
        // dotyk so zemou
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("GroundMoving"))
            jumping = false;
        
        // spadnutie do jamy
        if (other.gameObject.CompareTag("Hole")) 
            gameManager.gameOver = true;
        
        // narazenie do prisery
        if (other.gameObject.CompareTag("Monster")) {
            gameManager.hit = true;
            hitAudio.Play();
        }
        
        // poklad na konci
        if (other.gameObject.CompareTag("Treasure")) {
            pickCoinAudio.Play();
            foreach (var x in coins) 
                x.SetActive(false);
            gameManager.win = true;
            
        }
    }
    
    void OnTriggerEnter(Collider other) {
        // zbieranie minci
        if (other.gameObject.CompareTag ("PickUp")) {    
            pickCoinAudio.Play();
            other.gameObject.SetActive (false);
            gameManager.score++;
        }
        // prechod finishom
        if (other.gameObject.CompareTag ("Finish")) {
            finishAudio.Play();
            particleAudio.Play();
            gameManager.finished = true;
        }
    }

    // event vyvolany WSAD/sipkami => pohyb hraca
    void OnMove(InputValue movementValue) {
        // vektor pohybu z inputu
        Vector2 inputVector = movementValue.Get<Vector2>();
        // vytvorenie 3D vektora
        Vector3 inputVector3D = new Vector3(inputVector.x, 0, inputVector.y);
        // smer pohybu upraveny tak, aby bol relativny k otoceniu kamery
        moveVector = cameraTransform.TransformDirection(inputVector3D);
    }

    // event vyvolany ESC => zobrazi sa menu
    void OnDisplayMenu() { uiManager.DisplayMainMenu(); }
}
