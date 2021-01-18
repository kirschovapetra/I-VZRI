using System;
using UnityEngine;
using UnityEngine.Video;

// jumpscare na tv po prechode triggerom
public class TVJumpScare : MonoBehaviour {
    public VideoPlayer videoPlayer;
    private Boolean alreadyPlayed;
    
    // spusti sa video na TV
    private void OnTriggerEnter(Collider other) {
        if (!alreadyPlayed && other.CompareTag("Player")) {
            
            // kamera sa otaca za TV
            MouseLook_Custom.SetTransformToFollow(transform.Find("LookAtPoint").gameObject, 200f);

            videoPlayer.Play();
            alreadyPlayed = true;
        }
    }
}
