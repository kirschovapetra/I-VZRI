using UnityEngine;

// otacanie coinov
public class Rotator : MonoBehaviour {

    public float speed;
    void Update () { transform.Rotate(Vector3.up * speed * Time.deltaTime); }
}
