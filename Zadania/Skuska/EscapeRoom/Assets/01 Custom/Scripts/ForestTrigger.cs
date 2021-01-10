using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestTrigger : MonoBehaviour {
    public InteractiveObjectsContainer IOC;
    private AudioSource[] audioSources;
    void Start() {
        audioSources = IOC.forestAudio_multi.GetComponents<AudioSource>();
    }
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            foreach (var audioSrc in audioSources) {
                audioSrc.Play();
            }

        }
    
}
}
