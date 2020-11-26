using UnityEngine;
using System.Collections;
 
public class Movement : MonoBehaviour {
 
    public float jumpHeight = 5.0f;
    public float moveSpeed = 2.0f;
 
 
    Rigidbody2D playerRigidbody;
 
 
 
    public bool LeftPressed = false;
    public bool RightPressed = false;
 
    // Use this for initialization
    void Start () {
        playerRigidbody = GetComponent<Rigidbody2D>();
 
    }
 
    // Update is called once per frame
    void FixedUpdate () {
        
        if (LeftPressed == true){
            Left ();
        }
 
        if (RightPressed == true){
            Right ();
        }
 
    }
 
    public void Jump ()
    {
        
        playerRigidbody.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
            
    }
 
 
 
    void Left ()
    {
 
        transform.Translate(-Vector2.right * moveSpeed * Time.deltaTime);
 
    }
 
    public void onPointerDownLeftButton()
    {
        LeftPressed = true;
    }
    public void onPointerUpLeftButton()
    {
        LeftPressed = false;
    }
 
 
    public void Right ()
    {
 
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
 
    }
 
    public void onPointerDownRightButton()
    {
        RightPressed = true;
    }
 
    public void onPointerUpRightButton()
    {
        RightPressed = false;
    }
 
 
}