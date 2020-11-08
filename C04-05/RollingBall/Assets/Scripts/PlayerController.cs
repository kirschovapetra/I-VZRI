using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
public class PlayerController : MonoBehaviour
{
    public float speed;
    private Rigidbody rb;
    private float movementX;
    private float movementY;
    private int count;
    public TextMeshProUGUI countText; 
    public TextMeshProUGUI winText;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText(); 
        winText.text = "";
    }
 
    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
 
        movementX = movementVector.x;
        movementY = movementVector.y;
    }
 
    // Update is called once per frame
    void FixedUpdate () {
 
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
 
        rb.AddForce (movement*speed);
    }
    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag ("PickUp"))
        {
            other.gameObject.SetActive (false);
            count++;
            SetCountText(); 
        }
    }
    
    void SetCountText ()
    {
        countText.text = "Count: " + count.ToString ();
        if (count >= 12)
        {
            winText.text = "You Win!";
        }
    }
}
