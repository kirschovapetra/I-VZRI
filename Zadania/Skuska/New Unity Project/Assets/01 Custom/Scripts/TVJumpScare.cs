using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.Video;

public class TVJumpScare : MonoBehaviour {
    public VideoPlayer videoPlayer;
    private void OnTriggerEnter(Collider other) {
        if (!videoPlayer.isPlaying && other.CompareTag("Player")) {
            videoPlayer.Play();
        }
    }
}
