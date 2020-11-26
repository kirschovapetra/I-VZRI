using UnityEngine;
using UnityEngine.Audio;

//ovladanie hlasitosti
public class AudioManagerScript : MonoBehaviour {

    public float defaultVolume = 0.5f; 
    public AudioMixer mixer; 
    
    // prevod zo stupnice 0-1 na decibely
    private float LinearToDecibel(float linear) { return (linear != 0) ? 20.0f * Mathf.Log10(linear) : -144.0f; }
    
    //nastavenie hlasitosti v mixeri
    public void SetVolume() {
        float musicVolume = PlayerPrefs.GetFloat("music",defaultVolume);
        mixer.SetFloat("musicVolume", LinearToDecibel(musicVolume));
        
        float effectsVolume = PlayerPrefs.GetFloat("effects",defaultVolume);
        mixer.SetFloat("effectsVolume", LinearToDecibel(effectsVolume));
    }
}

