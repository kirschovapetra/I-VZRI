using UnityEngine;


// pohyb bodu nad playerom, na ktory sa pozera kamera
public class MoveLookAtPoint : MonoBehaviour {
    public Transform playerTransform;

    void Update() {
        Vector3 position = playerTransform.position;
        transform.position = new Vector3(position.x, position.y+3, position.z);
    }
}
