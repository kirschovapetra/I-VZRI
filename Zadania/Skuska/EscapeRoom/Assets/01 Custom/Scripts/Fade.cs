using UnityEngine;
using UnityEngine.UI;

// fade-in fade-out obrazovky a hudby
public class Fade : MonoBehaviour {
    public Image fadeImage;                // obrazok na canvas (cierny)
    private AudioManager audioManager;     // ovladanie zvuku
    
    void Start() {
        audioManager = GetComponent<AudioManager>();
        fadeImage.gameObject.SetActive(true);
        FadeIn();
    }

    // obraz -> cierna obrazovka
    public void FadeOut() {
        fadeImage.gameObject.SetActive(true);
        // najskor je cierny obrazok neviditelny (alpha = 0) 
        fadeImage.canvasRenderer.SetAlpha(0.0f);   
        // hudba sa postupne stisi
        StartCoroutine(audioManager.FadeMusic( 2.5f, 0.0f));
        // cierny obrazok sa zobrazuje
        fadeImage.CrossFadeAlpha(1.0f,2.5f,false);
    }
    
    private void HideImage() { fadeImage.gameObject.SetActive(false); }

    // cierna obrazovka -> obraz
    private void FadeIn() {
        // najskor je cierny obrazok viditelny (alpha = 1)
        fadeImage.canvasRenderer.SetAlpha(1.0f);
        // hudba sa postupne stisi
        StartCoroutine(audioManager.FadeMusic( 2.5f, 1.0f));
        // cierny obrazok sa skryva
        fadeImage.CrossFadeAlpha(0.0f, 2.5f, false);
        Invoke(nameof(HideImage), 2.55f);
    }
}

