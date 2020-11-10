using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideToSideMovement : MonoBehaviour {
    private Vector3 startPosition;
    public float distance;
    
    public float speed = 10.0f;
    public bool reverse = false;
    public string axis = "x";
    
    // Start is called before the first frame update
    void Start() {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update() {
        if (!reverse) {
            if (axis == "x") {
                float newX = startPosition.x + Mathf.PingPong(Time.time * speed, distance);
                transform.position = new Vector3(newX, transform.position.y, transform.position.z);
            }
            else if (axis == "y") {
                float newY = startPosition.y + Mathf.PingPong(Time.time * speed, distance);
                transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            }
            else if (axis == "z") {
                float newZ = startPosition.z + Mathf.PingPong(Time.time * speed, distance);
                transform.position = new Vector3(transform.position.x, transform.position.y, newZ);
            }
        }
        else {
            if (axis == "x") {
                float newX = startPosition.x - Mathf.PingPong(Time.time * speed, distance);
                transform.position = new Vector3(newX, transform.position.y, transform.position.z);
            }
            else if (axis == "y") {
                float newY = startPosition.y - Mathf.PingPong(Time.time * speed, distance);
                transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            }
            else if (axis == "z") {
                float newZ = startPosition.z - Mathf.PingPong(Time.time * speed, distance);
                transform.position = new Vector3(transform.position.x, transform.position.y, newZ);
            }
        }
    }
}
