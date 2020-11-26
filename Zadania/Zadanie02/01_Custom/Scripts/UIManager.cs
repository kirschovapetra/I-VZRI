using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// funkcionality UI
public class UIManager : MonoBehaviour {
    public PlayerInput cameraInput;                          // input system kamery
    public Rigidbody playerRigidbody;                        // rb playera
    
    // UI
    public Toggle musicToggle, effectsToggle;                
    public Slider musicSlider, effectsSlider;              
    public GameObject audioPanel, helpPanel, menuPanel;    
    
    private AudioManagerScript audioManagerScript;          //script na ovladanie zvuku

    void Start() {
        audioManagerScript = gameObject.GetComponent<AudioManagerScript>();
        
        // nastavenie hlasitosti na hodnoty ulozene v PlayerPrefs
        musicSlider.value = PlayerPrefs.GetFloat("music",audioManagerScript.defaultVolume);
        effectsSlider.value = PlayerPrefs.GetFloat("effects",audioManagerScript.defaultVolume);
        audioManagerScript.SetVolume();
    }

    private void Update() {
        
        // nastavenie checkboxov = nie su oznacene, ked je hlasitost 0
        if (musicSlider.value > 0.0f) {
            if (!musicToggle.isOn)
                musicToggle.isOn = true;
        }
        else 
            musicToggle.isOn = false;
        

        if (effectsSlider.value > 0.0f) {
            if (!effectsToggle.isOn)
                effectsToggle.isOn = true;
        }  
        else 
            effectsToggle.isOn = false;
        

    }
    
    // prepinanie zobrazenia panelov
    private void TogglePanel(GameObject visible, GameObject hidden) {
        visible.SetActive(true);
        hidden.SetActive(false);
    }

    // zobrazenie menu
    public void DisplayMainMenu() {
        cameraInput.enabled = false;
        playerRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        menuPanel.SetActive(true);
    }

    // navrat do hry
    public void ReturnToGame() {
        cameraInput.enabled = true;
        playerRigidbody.constraints = RigidbodyConstraints.None;
        menuPanel.SetActive(false);
    }

    // opakovanie hry
    public void RetryLevel() {
        cameraInput.enabled = true;
        playerRigidbody.constraints = RigidbodyConstraints.None;
        SceneManager.LoadScene("Game");
    }

    // ukoncenie hry
    public void Exit() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit ();
#endif
    
    }

    // zobrazenie a skrytie napovedy
    public void DisplayHelp() { TogglePanel(helpPanel,menuPanel); }
    public void HideHelp() { TogglePanel(menuPanel,helpPanel); }
    
    
    // zobrazenie a skrytie nastavenia zvuku
    public void DisplayAudioSettings() { TogglePanel(audioPanel,menuPanel); }
    public void HideAudioSettings() { TogglePanel(menuPanel,audioPanel); }
    
    
    //nastavenie hlasitosti hudby 
    public void UpdateMusicVolume() {
        PlayerPrefs.SetFloat("music", musicSlider.value); 
        audioManagerScript.SetVolume();
    }

    //nastavenie hlasitosti zvukovych efektov
    public void UpdateEffectsVolume() {
        PlayerPrefs.SetFloat("effects", effectsSlider.value); 
        audioManagerScript.SetVolume();
    }
		
    //nastavenie hlasitosti na default hodnotu (0.5)
    public void ResetVolume() {
        PlayerPrefs.SetFloat("music", audioManagerScript.defaultVolume);
        musicSlider.value = audioManagerScript.defaultVolume;
   
        PlayerPrefs.SetFloat("effects", audioManagerScript.defaultVolume);
        effectsSlider.value = audioManagerScript.defaultVolume;
        
        audioManagerScript.SetVolume();
    }

    // checkbox na hudbu - enable/disable
    public void ToggleMusic() {
        // zapnutie hudby - volume default (0.5)
        if (musicToggle.isOn) {
            PlayerPrefs.SetFloat("music", audioManagerScript.defaultVolume);
            musicSlider.value =  audioManagerScript.defaultVolume;
        }
        // vypnutie hudby - volume 0
        else {
            PlayerPrefs.SetFloat("music", 0.0f);
            musicSlider.value = 0.0f;
        }
        
        audioManagerScript.SetVolume();
    }

    // checkbox na efekty - enable/disable
    public void ToggleEffects() {
        // zapnutie efektov - volume default (0.5)
        if (effectsToggle.isOn) {
            PlayerPrefs.SetFloat("effects", audioManagerScript.defaultVolume);
            effectsSlider.value =  audioManagerScript.defaultVolume;
        }
        // vypnutie efektov - volume 0
        else {
            PlayerPrefs.SetFloat("effects", 0.0f);
            effectsSlider.value = 0.0f;
        }
        
        audioManagerScript.SetVolume();
    }
    
}
