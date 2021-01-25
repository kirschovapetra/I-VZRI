/************************** fade-in, fade-out efekty **************************/

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour {
    public Image fadeImage;                        // obrazok na canvas (cierny)
    private AudioManager audioManager;             // ovladanie zvuku
    public TextMeshProUGUI[] startGameMessages;    // text na zaciatku hry

    void Start() {
        audioManager = GetComponent<AudioManager>();
        fadeImage.gameObject.SetActive(true);
        FadeIn();                              
    }
    
    // zaciatok hry - fade in + text
    public IEnumerator FadeOutWithMessages() {
        FadeOut(music:false); // fade out obrazovky, hudba zostane hrat
        
        yield return new WaitForSeconds(4f);

        // zobrazuje sa po jednej massage
        foreach (var message in startGameMessages) {
            string tempMessage = message.text;
            message.gameObject.SetActive(true);
            
            // postupne zobrazovanie textu
            StartCoroutine(FadeInText(message));
            
            // delay podla dlzky textu
            yield return new WaitForSeconds(tempMessage.Length <= 20 ? 4f : 7f);
            
            message.gameObject.SetActive(false);
        }
        StartCoroutine(audioManager.FadeMusic( 3.5f, 0.0f));     // zvuk 1 -> 0
        StartCoroutine(GameManager.WaitAndLoadScene("Escape Room", 3.55f));    // prepnutie sceny
    }
    
    // obraz -> cierna obrazovka
    public void FadeOut(Image imgToFade = null, float time = 2.5f, bool music = true) {
        if (imgToFade == null) imgToFade = fadeImage;
        
        imgToFade.gameObject.SetActive(true);
        imgToFade.canvasRenderer.SetAlpha(0.0f);
        
        if (music) 
            StartCoroutine(audioManager.FadeMusic( time, 0.0f)); // zvuk 1 -> 0
        
        imgToFade.CrossFadeAlpha(1.0f,time,false);                // alpha obrazku 0 -> 1
    }
    
    // cierna obrazovka -> obraz
    private void FadeIn(Image imgToFade = null, float time = 2.5f, bool music = true) {
        if (imgToFade == null) imgToFade = fadeImage;
        
        imgToFade.canvasRenderer.SetAlpha(1.0f);
        if (music)
            StartCoroutine(audioManager.FadeMusic( time, 1.0f)); // zvuk 0 -> 1
        
        StartCoroutine(WaitAndHideImage( imgToFade, time+0.05f)); 
        imgToFade.CrossFadeAlpha(0.0f, time, false);              // alpha obrazku 1 -> 0
    }
    
    public IEnumerator FadeOutMultiple(Image img1, Image img2) {
        FadeOut(img1); // postupne sa ukaze end message
        yield return new WaitForSeconds(4f);
        FadeOut(img2, 1f); // postupne sa zacierni obrazovka
    }
    
    // odkryvanie textu
    private IEnumerator FadeInText(TextMeshProUGUI textToReveal) {
        string originalText = textToReveal.text;
        textToReveal.text = "";

        int currentLength = 0;
        while (currentLength < originalText.Length) {
            ++currentLength;
            textToReveal.text = originalText.Substring(0, currentLength);

            yield return new WaitForSeconds(0.07f);
        }
    }

    private IEnumerator WaitAndHideImage(Image imgToHide, float time) {
        yield return new WaitForSeconds(time);
        imgToHide.gameObject.SetActive(false);
    }

}

