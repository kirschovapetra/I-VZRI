using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// fade-in fade-out efekty
public class Fade : MonoBehaviour {
    public Image fadeImage;                // obrazok na canvas (cierny)
    private AudioManager audioManager;     // ovladanie zvuku

    void Start() {
        audioManager = GetComponent<AudioManager>();
        fadeImage.gameObject.SetActive(true);
    }
    
    // obraz -> cierna obrazovka
    public void FadeOut(Image imgToFade, float time) {
        imgToFade.gameObject.SetActive(true);
        imgToFade.canvasRenderer.SetAlpha(0.0f);
        StartCoroutine(audioManager.FadeMusic( time, 0.0f)); // zvuk 1 -> 0
        imgToFade.CrossFadeAlpha(1.0f,time,false);                // alpha obrazku 0 -> 1
    }
    
    public IEnumerator FadeOutMultiple(Image img1, Image img2) {
        FadeOut(img1, 2.5f); // postupne sa ukaze end message
        yield return new WaitForSeconds(4f);
        FadeOut(img2, 1f); // postupne sa zacierni obrazovka
    }
    
    // odkryvanie textu
    // src: https://gist.github.com/miguelSantirso/9647e8712bec3168bdfe9618db502870
    public IEnumerator FadeInText(TextMeshProUGUI textToReveal) {
        string originalText = textToReveal.text;
        textToReveal.text = "";

        int currentLength = 0;
        while (currentLength < originalText.Length) {
            ++currentLength;
            textToReveal.text = originalText.Substring(0, currentLength);

            yield return new WaitForSeconds(0.07f);
        }
    }
    
    // cierna obrazovka -> obraz
    public void FadeIn(Image imgToFade, float time) {
        imgToFade.canvasRenderer.SetAlpha(1.0f);
        StartCoroutine(audioManager.FadeMusic( time, 1.0f)); // zvuk 0 -> 1
        StartCoroutine(WaitAndHideImage( imgToFade, time+0.05f)); 
        imgToFade.CrossFadeAlpha(0.0f, time, false);              // alpha obrazku 1 -> 0
    }

    private IEnumerator WaitAndHideImage(Image imgToHide, float time) {
        yield return new WaitForSeconds(time);
        imgToHide.gameObject.SetActive(false);
    }

}

