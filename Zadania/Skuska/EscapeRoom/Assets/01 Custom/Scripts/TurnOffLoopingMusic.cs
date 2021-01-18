using System;
using UnityEngine;

public class TurnOffLoopingMusic : MonoBehaviour {
    private Boolean alreadyStopped;
    private void OnTriggerEnter(Collider other) {
        if (!alreadyStopped && other.CompareTag("Player")) {
            AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
            foreach (var audioSource in audioSources) {
                if(audioSource.outputAudioMixerGroup == null || audioSource.outputAudioMixerGroup.name.Equals("Effects"))
                    audioSource.Pause();
            }
            alreadyStopped = true;
        }
    }
}
