/**************** vypnutie hudobnych efektov a casomiery ****************/

using UnityEngine;

public class TurnOffLoopingMusic : MonoBehaviour {
    private bool alreadyStopped;

    void Start() { alreadyStopped = false; }

    private void OnTriggerEnter(Collider other) {
        // player presiel triggerom
        if (!alreadyStopped && other.CompareTag("Player")) {
            
            // loop cez vsetky hudobne efekty
            AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
            foreach (var audioSource in audioSources) {
                if(audioSource.outputAudioMixerGroup == null || audioSource.outputAudioMixerGroup.name.Equals("Effects"))
                    audioSource.Pause();
            }
            // hodiny sa zastavia
            Clock.stop = true;
            
            alreadyStopped = true;
        }
    }
}
