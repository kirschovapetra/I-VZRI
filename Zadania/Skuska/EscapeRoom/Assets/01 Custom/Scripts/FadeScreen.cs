using UnityEngine;
using UnityEngine.UI;


public class FadeScreen : MonoBehaviour {
    public Image fadeImage;
    
    void Start() {
        fadeImage.gameObject.SetActive(true);
        FadeOut();
    }

    public void FadeIn() {
        fadeImage.gameObject.SetActive(true);
        fadeImage.canvasRenderer.SetAlpha(0.0f);
        fadeImage.CrossFadeAlpha(1.0f,2.5f,false);
        
    }
    private void HideImage() {
        fadeImage.gameObject.SetActive(false);
    }

    public void FadeOut() {
        fadeImage.canvasRenderer.SetAlpha(1.0f);
        fadeImage.CrossFadeAlpha(0.0f,2.5f,false);
        Invoke(nameof(HideImage),2.55f);
    }
}

