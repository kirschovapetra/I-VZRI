using UnityEngine;

public class WindowJumpScare : MonoBehaviour {
    public GameObject zombie;
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            zombie.SetActive(true);
        }
    }
}
