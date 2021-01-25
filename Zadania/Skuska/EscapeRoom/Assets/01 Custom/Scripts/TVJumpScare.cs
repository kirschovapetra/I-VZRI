/*********************** jumpscare na tv po prechode triggerom ***********************/

using UnityEngine;
using UnityEngine.Video;

public class TVJumpScare : MonoBehaviour {
    public VideoPlayer videoPlayer;
    private bool alreadyPlayed;
    
    private void OnTriggerEnter(Collider other) {
        // player presiel triggerom -> spusti sa video na TV
        if (!alreadyPlayed && other.CompareTag("Player")) {
            videoPlayer.Play();
            alreadyPlayed = true;
        }
    }
}
