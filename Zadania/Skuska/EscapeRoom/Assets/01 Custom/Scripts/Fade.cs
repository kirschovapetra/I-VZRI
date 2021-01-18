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
    
    
    public void FadeOut() {
        fadeImage.gameObject.SetActive(true);
        fadeImage.canvasRenderer.SetAlpha(0.0f);
        StartCoroutine(audioManager.FadeMusic( 2.5f, 0.0f));
        fadeImage.CrossFadeAlpha(1.0f,2.5f,false);
    }
    
    private void HideImage() { fadeImage.gameObject.SetActive(false); }

    private void FadeIn() {
        fadeImage.canvasRenderer.SetAlpha(1.0f);
        StartCoroutine(audioManager.FadeMusic( 2.5f, 1.0f));
        fadeImage.CrossFadeAlpha(0.0f, 2.5f, false);
        Invoke(nameof(HideImage), 2.55f);
    }
}

