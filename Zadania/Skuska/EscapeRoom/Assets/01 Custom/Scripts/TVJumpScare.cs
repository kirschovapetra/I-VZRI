using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.Video;

public class TVJumpScare : MonoBehaviour {
    public VideoPlayer videoPlayer;
    private Boolean alreadyPlayed;
    private void OnTriggerEnter(Collider other) {
        if (!alreadyPlayed && other.CompareTag("Player")) {
            videoPlayer.Play();
            alreadyPlayed = true;
        }
    }
}
