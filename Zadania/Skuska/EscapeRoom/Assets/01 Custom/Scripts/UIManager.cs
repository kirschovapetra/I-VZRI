using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// GUI
public class UIManager : MonoBehaviour {

    public Boolean mainMenu;
    public GameObject menuPanel;
    public GameObject tutorialPanel;
    public GameObject settingsPanel;
    
    // audio
    private AudioManager audioManager;
    private Slider musicSlider;
    private Slider effectsSlider;

    private TextMeshProUGUI musicPercent;
    private TextMeshProUGUI effectsPercent;
    
    // screen resolution
    private Toggle fullScreenToggle;
    private TMP_Dropdown resolutionDropdown;
    private Boolean changed;
    
    void Start() {
        audioManager = GetComponent<AudioManager>();

        // slidery
        musicSlider = settingsPanel.transform.Find("MusicSlider").GetComponent<Slider>();
        effectsSlider = settingsPanel.transform.Find("EffectsSlider").GetComponent<Slider>();
        // percenta - text
        musicPercent = settingsPanel.transform.Find("MusicPercentText").GetComponent<TextMeshProUGUI>();
        effectsPercent = settingsPanel.transform.Find("EffectsPercentText").GetComponent<TextMeshProUGUI>();
        // screen resolution
        fullScreenToggle = settingsPanel.transform.Find("FullScreenToggle").GetComponent<Toggle>();
        resolutionDropdown = settingsPanel.transform.Find("ResolutionDropdown").GetComponent<TMP_Dropdown>();
        
        // nastavenie hlasitosti na hodnoty ulozene v PlayerPrefs
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume",audioManager.defaultVolume);
        effectsSlider.value = PlayerPrefs.GetFloat("effectsVolume",audioManager.defaultVolume);
        audioManager.SetVolume();

        if (mainMenu)
            ResetSettings();
    }

    private void ChangeDropdownVisibility(Boolean interactable) {
        resolutionDropdown.interactable = interactable;

        if (interactable) {
            resolutionDropdown.transform.Find("Label").GetComponent<TextMeshProUGUI>().alpha = 1.0f;
            resolutionDropdown.transform.Find("Arrow").GetComponent<Image>().color = Color.white;
        }
        else {
            resolutionDropdown.transform.Find("Label").GetComponent<TextMeshProUGUI>().alpha = 0.2f;
            resolutionDropdown.transform.Find("Arrow").GetComponent<Image>().color = new Color(0.34f, 0.34f, 0.34f);
        }
    }

    private void SetAudioText() {
        // nastavenie textu - percento hlasitosti
        musicPercent.text = Math.Round(musicSlider.value*100,0) + "%";
        effectsPercent.text = Math.Round(effectsSlider.value*100,0) + "%";
    }
    
    private void Update() {

        SetAudioText();
        
        // prepnutie full screen / windowed mode
        if(fullScreenToggle.isOn) {
            ChangeDropdownVisibility(false);
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else {
            ChangeDropdownVisibility(true);
            Screen.fullScreenMode = FullScreenMode.Windowed;
            if (!changed) {
                ChangeResolution();
                changed = true;
            }
        }

        // zobrazenie/skrytie menu po stlaceni 'Esc' (iba pocas hry)
        if (!mainMenu && Input.GetKeyDown(KeyCode.Escape))
            ToggleMenuOverlay();
    }


    // zobrazenie/skrytie menu (maleho okna)
    public void ToggleMenuOverlay() {
        Boolean menuVisible = !GameManager.exitingToMenu && !menuPanel.activeSelf;
        
        // hra je pauznuta, ked sa zobrazuje menu
        if (menuVisible) 
            GameManager.PauseGame();    
        else 
            GameManager.ResumeGame();
        
        menuPanel.SetActive(menuVisible);
    }

    // spustenie hry
    public void StartGame() {
        GetComponent<Fade>().FadeOut();
        Invoke(nameof(LoadEscapeRoom), 2.55f);
    }

    private void LoadEscapeRoom() {
        SceneManager.LoadScene("Escape Room");
    }

    private void LoadMainMenu() {
        SceneManager.LoadScene("Main Menu");
    }
    // ukoncenie hry
    public void Exit() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit ();
#endif
    
    }
    
    // hlavne menu
    public void ExitToMainMenu() {
        GameManager.exitingToMenu = true;
        // GameManager.ResumeGame();
        GetComponent<Fade>().FadeOut();
        Invoke(nameof(LoadMainMenu), 2.55f);
        ToggleMenuOverlay();

    }

    // prepinanie zobrazenia objektov
    private void ToggleObjects(GameObject visible, GameObject hidden) {
        visible.SetActive(true);
        hidden.SetActive(false);
    }
    
    // zobrazenie a skrytie tutorialu
    public void ShowTutorial() { ToggleObjects(tutorialPanel,menuPanel); }
    public void HideTutorial() { ToggleObjects(menuPanel,tutorialPanel); }
    
    public void ShowTutorial_Main() { ToggleObjects(tutorialPanel,settingsPanel); }
    public void HideTutorial_Main() { tutorialPanel.gameObject.SetActive(false); }
    
    // zobrazenie a skrytie nastaveni
    public void ShowSettings() { ToggleObjects(settingsPanel,menuPanel); }
    public void HideSettings() { ToggleObjects(menuPanel,settingsPanel); }
    
    public void ShowSettings_Main() { ToggleObjects(settingsPanel,tutorialPanel); }
    public void HideSettings_Main() { settingsPanel.gameObject.SetActive(false); }
    
    //nastavenie hlasitosti hudby 
    public void UpdateMusicVolume() {
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value); 
        audioManager.SetVolume();
    }

    //nastavenie hlasitosti zvukovych efektov
    public void UpdateEffectsVolume() {
        PlayerPrefs.SetFloat("effectsVolume", effectsSlider.value); 
        audioManager.SetVolume();
    }
		
    // default nastavenia
    public void ResetSettings() {
        PlayerPrefs.SetFloat("musicVolume", audioManager.defaultVolume);
        musicSlider.value = audioManager.defaultVolume;
   
        PlayerPrefs.SetFloat("effectsVolume", audioManager.defaultVolume);
        effectsSlider.value = audioManager.defaultVolume;
        
        audioManager.SetVolume();

        fullScreenToggle.isOn = true;
        
    }

    // zmena rozlisenia obrazovky
    public void ChangeResolution() {
        var resolution = resolutionDropdown.options[resolutionDropdown.value];
        string[] splitString = resolution.text.Split('x');
        // print(splitString[0] + " "+ splitString[1]);
        Screen.SetResolution(Int32.Parse(splitString[0]), Int32.Parse(splitString[1]), false);
        
    }
}
