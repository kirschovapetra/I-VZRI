/******************** trigger na konci hry ************************/

using UnityEngine;
using UnityEngine.UI;

public class EndTrigger : MonoBehaviour {
    public Fade fade;
    public Image endMessage;
    private void OnTriggerEnter(Collider other) {
        // player presiel triggerom
        if (other.CompareTag("Player")) {
            other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition; // hrac sa nemoze hybat
            StartCoroutine(fade.FadeOutMultiple(endMessage,fade.fadeImage));            // fade out obrazovky
            StartCoroutine(GameManager.WaitAndLoadScene("Main Menu", 5f));         // prechod do menu
        }
    }
}
