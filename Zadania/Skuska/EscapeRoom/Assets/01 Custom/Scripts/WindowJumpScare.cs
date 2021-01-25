/********************** jumpscare na okne po prechode triggerom **********************/

using UnityEngine;

public class WindowJumpScare : MonoBehaviour {
    public GameObject zombie;
    private bool alreadyPlayed;
    private void OnTriggerEnter(Collider other) {
        if (!alreadyPlayed && other.CompareTag("Player")) {
            // kamera sa otaca na okno
            MouseLook_Custom.SetTransformToFollow(transform.Find("LookAtPoint").gameObject, 200f);
            zombie.SetActive(true);
            alreadyPlayed = true;
        }
        
    }
}
