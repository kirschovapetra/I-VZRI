using UnityEngine;

// pohyb objektov zo strany na stranu
// zdroj: https://www.youtube.com/watch?v=k_QrWF_jGFE&ab_channel=ThePhantomGameDesigns
public class SideToSideMovement : MonoBehaviour {
    
    public float speed = 10.0f;      // rychlost pohybu
    public bool reverse = false;     // v akom smere sa pohybuje (true => suradnice sa zvacsuju a naopak)
    public string axis = "x";        // os pohybu
    
    // zaciatocna pozicia, dlzka drahy pohybu
    private Vector3 startPosition;     
    public float distance;

    void Start() {
        startPosition = transform.position;     // aktualna pozicia
    }

    void Update() {
        Vector3 currentPosition = transform.position;
        
        // pohyb dozadu - dopredu
        if (reverse) {
            if (axis == "x") {
                float newX = startPosition.x - Mathf.PingPong(Time.time * speed, distance);
                transform.position = new Vector3(newX, currentPosition.y, currentPosition.z);
            }
            else if (axis == "y") {
                float newY = startPosition.y - Mathf.PingPong(Time.time * speed, distance);
                transform.position = new Vector3(currentPosition.x, newY, currentPosition.z);
            }
            else if (axis == "z") {
                float newZ = startPosition.z - Mathf.PingPong(Time.time * speed, distance);
                transform.position = new Vector3(currentPosition.x, currentPosition.y, newZ);
            }
        }
        
        // pohyb dopredu - dozadu
        else {
            if (axis == "x") {
                float newX = startPosition.x + Mathf.PingPong(Time.time * speed, distance);
                transform.position = new Vector3(newX, transform.position.y, currentPosition.z);
            }
            else if (axis == "y") {
                float newY = startPosition.y + Mathf.PingPong(Time.time * speed, distance);
                transform.position = new Vector3(transform.position.x, newY, currentPosition.z);
            }
            else if (axis == "z") {
                float newZ = startPosition.z + Mathf.PingPong(Time.time * speed, distance);
                transform.position = new Vector3(transform.position.x, currentPosition.y, newZ);
            }
            
           
        }
    }
}
