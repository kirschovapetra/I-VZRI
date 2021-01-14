using UnityEngine;
using UnityEngine.SceneManagement;

// trigger na konci hry
public class EndTrigger : MonoBehaviour {
    public Fade fade;
    private void OnTriggerEnter(Collider other) {
        
        if (other.CompareTag("Player")) {
            fade.FadeOut();    // postupne sa zacierni obrazovka
            other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition; // hrac sa nemoze hybat
            SceneManager.LoadScene("Main Menu");
            //GameManager.PauseGame();
        }
    }
}
