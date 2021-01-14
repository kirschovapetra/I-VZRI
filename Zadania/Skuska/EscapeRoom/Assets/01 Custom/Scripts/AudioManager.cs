using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

//ovladanie zvuku
public class AudioManager: MonoBehaviour {

    public float defaultVolume = 1.0f; 
    public AudioMixer mixer;

    // prevod zo stupnice 0-1 na decibely
    private float LinearToDecibel(float linear) { return (linear != 0) ? 20.0f * Mathf.Log10(linear) : -144.0f; }
    
    //nastavenie hlasitosti v mixeri
    public void SetVolume() {
        float musicVolume = PlayerPrefs.GetFloat("musicVolume",defaultVolume);
        mixer.SetFloat("musicVolume", LinearToDecibel(musicVolume));
        
        float effectsVolume = PlayerPrefs.GetFloat("effectsVolume",defaultVolume);
        mixer.SetFloat("effectsVolume", LinearToDecibel(effectsVolume));
    }
    
    // fade-in/out hudby
    //src: https://gamedevbeginner.com/how-to-fade-audio-in-unity-i-tested-every-method-this-ones-the-best/
    public IEnumerator FadeMusic(float duration, float targetVolume) {
        float currentTime = 0;
        float currentVol;
        mixer.GetFloat("masterVolume", out currentVol);
        currentVol = Mathf.Pow(10, currentVol / 20);
        float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);

        while (currentTime < duration) {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
            mixer.SetFloat("masterVolume", Mathf.Log10(newVol) * 20);
            yield return null;
        }
    }
}

