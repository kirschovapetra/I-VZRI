using UnityEngine;

// trigger na konci hry
public class EndTrigger : MonoBehaviour {
    public Fade fade;
    private void OnTriggerEnter(Collider other) {
        
        if (other.CompareTag("Player")) {
            other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition; // hrac sa nemoze hybat
            fade.FadeOut();    // postupne sa zacierni obrazovka
            StartCoroutine(GameManager.WaitAndLoadScene("Main Menu", 2.5f));
        }
    }
}
