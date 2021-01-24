using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// trigger na konci hry
public class EndTrigger : MonoBehaviour {
    public Fade fade;
    public Image endMessage;
    private void OnTriggerEnter(Collider other) {
        
        if (other.CompareTag("Player")) {
            other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition; // hrac sa nemoze hybat
            StartCoroutine(fade.FadeOutMultiple(endMessage,fade.fadeImage));
            StartCoroutine(GameManager.WaitAndLoadScene("Main Menu", 5f)); // prechod do menu
        }
    }
}
