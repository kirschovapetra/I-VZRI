using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

//ovladanie zvuku
public class AudioManager: MonoBehaviour {

    public float defaultVolume = 1.0f; 
    public AudioMixer mixer;

    // prevod zo stupnice 0-1 na decibely
    private float LinearToDecibel(float linear) { return (linear != 0) ? 20.0f * Mathf.Log10(linear) : -144.0f; }
    private float DecibelToLinear(float decibel) { return Mathf.Pow(10, decibel / 20); }
    
    //nastavenie hlasitosti v mixeri
    public void SetVolume() {
        float musicVolume = PlayerPrefs.GetFloat("musicVolume",defaultVolume);
        mixer.SetFloat("musicVolume", LinearToDecibel(musicVolume));
        
        float effectsVolume = PlayerPrefs.GetFloat("effectsVolume",defaultVolume);
        mixer.SetFloat("effectsVolume", LinearToDecibel(effectsVolume));
        mixer.SetFloat("jumpscaresVolume", LinearToDecibel(effectsVolume)+5);
        
    }
    
    // fade-in/out hudby
    //src: https://gamedevbeginner.com/how-to-fade-audio-in-unity-i-tested-every-method-this-ones-the-best/
    public IEnumerator FadeMusic(float duration, float targetVolume) {
        // rozsah hodnot 0.001-1
        targetVolume = Mathf.Clamp(targetVolume, 0.0001f, 1);
        
        mixer.GetFloat("masterVolume", out float currentVolumeDecibel);
        float currentVolumeLinear = DecibelToLinear(currentVolumeDecibel);
        
        float currentTime = 0;
        while (currentTime < duration) {
            currentTime += Time.deltaTime;
            float newVolume = Mathf.Lerp(currentVolumeLinear, targetVolume, currentTime / duration);
            mixer.SetFloat("masterVolume", LinearToDecibel(newVolume));
            yield return null;
        }
    }
}

