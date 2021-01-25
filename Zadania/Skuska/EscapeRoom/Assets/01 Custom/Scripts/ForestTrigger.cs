/********************* trigger pred vstupom do lesa *********************/

using UnityEngine;

public class ForestTrigger : MonoBehaviour {
    [Header("Globálne premenné")]
    public GlobalObjectsContainer GOC; 
    private AudioSource[] audioSources;
    void Start() { audioSources = GOC.forestAudio_multi.GetComponents<AudioSource>(); }
    private void OnTriggerEnter(Collider other) {
        // spustenie hudobnych efektov
        if (other.CompareTag("Player")) {
            foreach (var audioSrc in audioSources) 
                audioSrc.Play();
        }
    }
}
