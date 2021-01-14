using UnityEngine;

// jumpscare na okne po prechode triggerom
public class WindowJumpScare : MonoBehaviour {
    public GameObject zombie;
    
    // zombie
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            zombie.SetActive(true);
        }
    }
}
